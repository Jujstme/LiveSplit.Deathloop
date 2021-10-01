using LiveSplit.ComponentUtil;
using System;

namespace LiveSplit.Deathloop
{
    class GameVariables
    {
        internal readonly string GameName = "DEATHLOOP";
        internal readonly string[] ExeName = new string[] { "Deathloop" };
        internal readonly byte refreshRate = 60;
        internal MemoryWatcherList watchers = new MemoryWatcherList();

        // Internal variables
        internal string OLD_map = "";
        internal string CURRENT_map = "";
        internal bool OLD_isLoading = false;
        internal bool CURRENT_isLoading = false;
    }

    partial class Component
    {
        private bool Init()
        {
            var scanner = new SignatureScanner(game, game.MainModule.BaseAddress, game.MainModule.ModuleMemorySize);
            IntPtr ptr;
            vars.watchers = new MemoryWatcherList();

            ptr = scanner.Scan(new SigScanTarget(3,
                "F0 FF 0D ????????", // lock dec [Deathloop.exe+3DFB810]  <----
                "49 8B 3E"));  // mov rdi,[r14]
            if (ptr == IntPtr.Zero) return false;
            vars.watchers.Add(new MemoryWatcher<bool>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr))) { Name = "isLoading", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });

            ptr = scanner.Scan(new SigScanTarget(2,
                "83 3D ???????? 00", // cmp dword ptr [Deathloop.exe+30C8300],00  <----
                "0F94 C0",           // sete al
                "BE 0C000000"));     // mov esi,0000000C
            if (ptr == IntPtr.Zero) return false;
            vars.watchers.Add(new MemoryWatcher<bool>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr))) { Name = "isLoading2", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr) + 0x4A48)) { Name = "isConnectingOnline", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });

            ptr = scanner.Scan(new SigScanTarget(3,
                "48 8B 0D ????????", // mov rcx,[Deathloop.exe+30CCD00]  <----
                "4C 89 74 24 48"));  // mov [rsp+48],r14
            if (ptr == IntPtr.Zero) return false;
            vars.watchers.Add(new StringWatcher(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x3E08), 255) { Name = "gameMapCode" });
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x3CE0)) { Name = "someLoadFlag", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });

            ptr = scanner.Scan(new SigScanTarget(3,
                "48 8B 05 ????????", // mov rax,[Deathloop.exe+30C8230]   <----
                "48 8B C8",          // mov rcx,rax
                "48 85 C9",          // test rcx,rcx
                "74 2A"));           // je Deathloop.exe+16F64AF
            if (ptr == IntPtr.Zero) return false;
            vars.watchers.Add(new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x2E0, 0x37C8, 0x0, 0xA0, 0x1F0, 0x84)) { Name = "yPos", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });
            vars.watchers.Add(new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x2E0, 0x37C8, 0x0, 0xA0, 0x1F0, 0x88)) { Name = "zPos", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });

            vars.watchers.UpdateAll(game);
            return true;
        }

        private void update()
        {
            vars.watchers.UpdateAll(game);
            vars.OLD_map = vars.CURRENT_map;
            if (((string)vars.watchers["gameMapCode"].Current).Contains("campaign")) vars.CURRENT_map = ((string)vars.watchers["gameMapCode"].Current).Substring(((string)vars.watchers["gameMapCode"].Current).LastIndexOf("/") + 1).Replace(".map", "");
            vars.OLD_isLoading = vars.CURRENT_isLoading;
            vars.CURRENT_isLoading = (bool)vars.watchers["isLoading"].Current || (bool)vars.watchers["isLoading2"].Current ||
                                     (((byte)vars.watchers["someLoadFlag"].Current & (1 << 0)) != 0) ||
                                     (vars.CURRENT_map == "menu" && (byte)vars.watchers["isConnectingOnline"].Current != 20 && (byte)vars.watchers["isConnectingOnline"].Current != 21);
        }

        private void start()
        {
            if (!settings.runStart) return;

            // Regular Any% start
            if (vars.CURRENT_map == "tutorial_01_p" && vars.OLD_isLoading && !vars.CURRENT_isLoading && (bool)vars.watchers["isLoading2"].Old)
            {
                _timer.CurrentState.Run.Offset = TimeSpan.FromSeconds(-51.5);
                _timer.Start();
                _timer.CurrentState.Run.Offset = TimeSpan.Zero;
                return;
            }

            // Final Loop starts at Fristad's Rock
            if (settings.runStartLastLoop && vars.CURRENT_map == "wharf_01" && vars.OLD_isLoading && !vars.CURRENT_isLoading && (bool)vars.watchers["isLoading2"].Old)
            {
                _timer.CurrentState.Run.Offset = TimeSpan.Zero;
                _timer.Start();
                return;
            }
        }

        private void isLoading()
        {
            _timer.CurrentState.IsGameTimePaused = vars.CURRENT_isLoading;
        }

        private void gameTime()
        {
            // _timer.CurrentState.SetGameTime(TimeSpan.FromSeconds(0));
        }

        private void resetLogic()
        {
            // _timer.Reset();
        }

        private void splitLogic()
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

            if (!vars.CURRENT_isLoading && vars.CURRENT_map == "upper_antenna_p" && (float)vars.watchers["yPos"].Current > -66f && (float)vars.watchers["zPos"].Current <= 185.7f && (float)vars.watchers["zPos"].Old > 185.7f && settings.MapVoid)
            {
                _timer.Split();
                return;
            }
        }
    }
}
