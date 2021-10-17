using LiveSplit.ComponentUtil;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LiveSplit.Deathloop
{
    class Watchers : MemoryWatcherList
    {
        private MemoryWatcher<bool> isLoading { get; }
        public MemoryWatcher<bool> isLoading2 { get; }
        public MemoryWatcher<byte> isConnectingOnline { get; }
        private StringWatcher gameMapCode { get; }
        private MemoryWatcher<byte> gameMapCode_byte { get; }
        private MemoryWatcher<byte> someLoadFlag { get; }
        public MemoryWatcher<float> yPos { get; }
        public MemoryWatcher<float> zPos { get; }

        // FakeMemoryWatchers
        public FakeMemoryWatcher<string> Map => new FakeMemoryWatcher<string>(
            this.gameMapCode_byte.Old != 0 ? (this.gameMapCode.Old.Contains("campaign") ? this.gameMapCode.Old.Substring(this.gameMapCode.Old.LastIndexOf("/") + 1).Replace(".map", "") : "") : "",
            this.gameMapCode_byte.Current != 0 ? (this.gameMapCode.Current.Contains("campaign") ? this.gameMapCode.Current.Substring(this.gameMapCode.Current.LastIndexOf("/") + 1).Replace(".map", "") : "") : "");

        public FakeMemoryWatcher<bool> LoadPause => new FakeMemoryWatcher<bool>(
            (this.isLoading.Old && this.Map.Old != Maps.Menu) || this.isLoading2.Old || (this.someLoadFlag.Old & 1) != 0 || (this.Map.Old == Maps.Menu && this.isConnectingOnline.Old < 20),
            (this.isLoading.Current && this.Map.Current != Maps.Menu) || this.isLoading2.Current || (this.someLoadFlag.Current & 1) != 0 || (this.Map.Current == Maps.Menu && this.isConnectingOnline.Current < 20));


        public Watchers(Process game)
        {
            var scanner = new SignatureScanner(game, game.MainModuleWow64Safe().BaseAddress, game.MainModuleWow64Safe().ModuleMemorySize);
            IntPtr ptr;

            // Basic checks
            if (!game.Is64Bit()) throw new Exception();
            ptr = scanner.Scan(new SigScanTarget("44 45 41 54 48 4C 4F 4F 50 20")); if (ptr == IntPtr.Zero) throw new Exception();

            ptr = scanner.Scan(new SigScanTarget(3,
                "F0 FF 0D ????????", // lock dec [Deathloop.exe+3DFB810]  <----
                "49 8B 3E"));  // mov rdi,[r14]
            if (ptr == IntPtr.Zero) throw new Exception();
            this.isLoading = new MemoryWatcher<bool>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr))) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            ptr = scanner.Scan(new SigScanTarget(2,
                "83 3D ???????? 00", // cmp dword ptr [Deathloop.exe+30C8300],00  <----
                "0F94 C0",           // sete al
                "BE 0C000000"));     // mov esi,0000000C
            if (ptr == IntPtr.Zero) throw new Exception();
            this.isLoading2 = new MemoryWatcher<bool>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr))) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            ptr = scanner.Scan(new SigScanTarget(3,
                "4C 89 35 ????????",    // mov [Deathloop.exe+30CCD48],r14  <----
                "44 89 35 ????????",    // mov [Deathloop.exe+30CCD50],r14d
                "4C 89 35 ????????"));  // mov [Deathloop.exe+30CCD58],r14
            if (ptr == IntPtr.Zero) throw new Exception();
            this.isConnectingOnline = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr))) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            ptr = scanner.Scan(new SigScanTarget(3,
                "0F B6 05 ????????", // movzx eax,byte ptr [Deathloop.exe+30D0B20]  <----
                "D0 E8"));           // shr al,1
            if (ptr == IntPtr.Zero) throw new Exception();
            this.gameMapCode = new StringWatcher(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x18), 255);
            this.gameMapCode_byte = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x18)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            ptr = scanner.Scan(new SigScanTarget(14,
                "48 C7 05 ????????????????",     // mov qword ptr [Deathloop.exe+30D6B80],00000012
                "44 89 35 ????????"));           // mov [Deathloop.exe+30D0A10],r14d  <----
            if (ptr == IntPtr.Zero) throw new Exception();
            this.someLoadFlag = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr))) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            ptr = scanner.Scan(new SigScanTarget(3,
                "48 8B 2D ????????", // mov rbp,[Deathloop.exe+30C82B0]  <----
                "44 8B 07"));        // mov r8d,rdi
            if (ptr == IntPtr.Zero) throw new Exception();
            int posOffset;
            switch (game.MainModuleWow64Safe().ModuleMemorySize)
            {
                case 0x22363000: case 0x22CAB000: case 0x23D93000: posOffset = 0xA0; break;
                default: posOffset = 0xA8; break;
            }
            this.yPos = new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x2F8, 0x0, 0x0, 0x98, posOffset, 0x1F0, 0x84)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            this.zPos = new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x2F8, 0x0, 0x0, 0x98, posOffset, 0x1F0, 0x88)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }
    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed { get; }
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
            this.Changed = (object)old != (object)current;
        }
    }

    static class Maps
    {
        internal const string UpdaamMorning = "uppercity_01_p";
        internal const string UpdaamNoon = "uppercity_02_p";
        internal const string UpdaamAfternoon = "uppercity_03_p";
        internal const string UpdaamEvening = "uppercity_04_p";

        internal const string KarlsBayMorning = "wharf_01";
        internal const string KarlsBayNoon = "wharf_02";
        internal const string KarlsBayAfternoon = "wharf_03";
        internal const string KarlsBayEvening = "wharf_04";

        internal const string FristadRockMorning = "island_01_p";
        internal const string FristadRockNoon = "island_02_p";
        internal const string FristadRockAfternoon = "island_03_p";
        internal const string FristadRockEvening = "island_04_p";

        internal const string ComplexMorning = "antenna_01_p";
        internal const string ComplexNoon = "antenna_02_p";
        internal const string ComplexAfternoon = "antenna_03_p";
        internal const string ComplexEvening = "antenna_04_p";

        internal const string Tutorial = "tutorial_01_p";
        internal const string AntennaRak = "upper_antenna_p";
        internal const string Menu = "menu";
        internal const string InvalidMap = "";
    }
}
