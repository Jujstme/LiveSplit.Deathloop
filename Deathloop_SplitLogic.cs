using System;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Deathloop
{
    class GameVariables
    {
        public string GameName = "DEATHLOOP";
        public string ExeName = "Deathloop";
        public byte refreshRate = 60;
        public MemoryWatcherList watchers = new MemoryWatcherList();

        // Game variables
        public MemoryWatcher<bool> isLoading, isLoading2;
        public MemoryWatcher<byte> isConnectingOnline;
        public StringWatcher gameMapCode;
        public MemoryWatcher<byte> someLoadFlag; // menuConnected;
        //public MemoryWatcher<long> someOtherLoadFlag;
        public MemoryWatcher<float> yPos, zPos;

        // Internal variables
        public string OLD_map = "";
        public string CURRENT_map = "";
        public bool OLD_isLoading = false;
        public bool CURRENT_isLoading = false;
    }
    partial class Component
    {
        private void Init()
        {
            var scanner = new SignatureScanner(game, game.MainModule.BaseAddress, game.MainModule.ModuleMemorySize);
            IntPtr ptr = IntPtr.Zero;

            
            ptr = scanner.Scan(new SigScanTarget(3,
                "F0 FF 0D ????????", // lock dec [Deathloop.exe+3DFB810]  <----
                "49 8B 3E"));  // mov rdi,[r14]
            vars.isLoading = new MemoryWatcher<bool>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr)));

            ptr = scanner.Scan(new SigScanTarget(2,
                "83 3D ???????? 00", // cmp dword ptr [Deathloop.exe+30C8300],00  <----
                "0F94 C0",           // sete al
                "BE 0C000000"));     // mov esi,0000000C
            vars.isLoading2 = new MemoryWatcher<bool>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr)));
            vars.isConnectingOnline = new MemoryWatcher<byte>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr) + 0x4A48));

            /*
                        ptr = scanner.Scan(new SigScanTarget(4,
                           "FF C8",             // dec eax
                           "89 05 ????????",    // mov [Deathloop.exe+5A2BA98],eax  <----
                           "48 8D 4C 24 40"));  // lea rcx,[rsp+40]
                        vars.isConnectingOnline = new MemoryWatcher<bool>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr)));
            */

            ptr = scanner.Scan(new SigScanTarget(3,
                "48 8B 0D ????????", // mov rcx,[Deathloop.exe+30CCD00]  <----
                "4C 89 74 24 48"));  // mov [rsp+48],r14
            vars.gameMapCode = new StringWatcher(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x3E08), 255);
            vars.someLoadFlag = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x3CE0));
            // vars.menuConnected = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x9696C4));

            ptr = scanner.Scan(new SigScanTarget(3,
                "48 8B 1D ????????", // mov rbx,[Deathloop.exe+2D60E00]   <----
                "48 8B F9",          // mov rdi,rcx
                "48 85 DB",          // test rbx,rbx
                "74 41"));           // je Deathloop.exe+B9E13A
            // vars.someOtherLoadFlag = new MemoryWatcher<long>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr)));
            vars.yPos = new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) - 0x1778, 0x8, 0x8, 0x98, 0xA0, 0x1F0, 0x84)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            vars.zPos = new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) - 0x1778, 0x8, 0x8, 0x98, 0xA0, 0x1F0, 0x88)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            vars.watchers = new MemoryWatcherList { vars.isLoading, vars.isLoading2, vars.someLoadFlag, vars.isConnectingOnline, vars.gameMapCode, vars.yPos, vars.zPos };

            switch (settings.disableOffset)
            {
                case true:
                    _timer.CurrentState.Run.Offset = TimeSpan.Zero;
                    break;
                case false:
                    _timer.CurrentState.Run.Offset = TimeSpan.FromSeconds(-51.5);
                    break;
            }
            
        }

        void update()
        {
            vars.watchers.UpdateAll(game);
            vars.OLD_map = vars.CURRENT_map;
            if (vars.gameMapCode.Current.Contains("campaign")) vars.CURRENT_map = vars.gameMapCode.Current.Substring(vars.gameMapCode.Current.LastIndexOf("/") + 1).Replace(".map", "");
            vars.OLD_isLoading = vars.CURRENT_isLoading;
            vars.CURRENT_isLoading = vars.isLoading.Current || vars.isLoading2.Current || 
                                     ((vars.someLoadFlag.Current & (1 << 0)) != 0) ||
                                     (vars.CURRENT_map == "menu" && vars.isConnectingOnline.Current != 20);
        }

        void start()
        {
            if (!settings.runStart) return;
            if (vars.CURRENT_map == "tutorial_01_p" && vars.OLD_isLoading && !vars.CURRENT_isLoading)
            {
                _timer.Start();
                return;
            }
        }
        void isLoading()
        {
            _timer.CurrentState.IsGameTimePaused = vars.CURRENT_isLoading;
        }

        void gameTime()
        {
            // _timer.CurrentState.SetGameTime(TimeSpan.FromSeconds(0));
        }

        void resetLogic()
        {
            // _timer.Reset();
        }

        void splitLogic()
        {
            if (!settings.enableSplitting) return;
            if (vars.CURRENT_map != vars.OLD_map && vars.OLD_map != "" && vars.CURRENT_map == "menu" && settings.MapLeave)
            {
                _timer.Split();
                return;
            }

            if (vars.CURRENT_map != vars.OLD_map && vars.OLD_map != "" && vars.CURRENT_map == "upper_antenna_p" && settings.MapAntenna)
            {
                _timer.Split();
                return;
            }

            if (!vars.CURRENT_isLoading && vars.CURRENT_map == "upper_antenna_p" && vars.yPos.Current > -66f && vars.zPos.Current <= 185.7f && vars.zPos.Old > 185.7f && settings.MapVoid)
            {
                _timer.Split();
                return;
            }
        }
    }
}
