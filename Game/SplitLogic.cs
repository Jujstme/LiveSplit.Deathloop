using System;
using System.Diagnostics;
using System.Linq;

namespace LiveSplit.Deathloop
{
    class SplittingLogic
    {
        private Process game;
        private Watchers watchers;

        public delegate void StartTriggerEventHandler(object sender, StartTrigger type);
        public event StartTriggerEventHandler OnStartTrigger;

        public delegate void GameTimeTriggerEventHandler(object sender, bool type);
        public event GameTimeTriggerEventHandler OnGameTimeTrigger;

        public delegate void SplitTriggerEventHandler(object sender, SplitTrigger type);
        public event SplitTriggerEventHandler OnSplitTrigger;

        public void Update()
        {
            if (game == null || game.HasExited) { if (!HookGameProcess()) return; }
            watchers.UpdateAll(game);
            Start();
            //IsLoading();
            GameTime();
            //ResetLogic();
            Split();
        }

        void Start()
        {
            // Starting Beach map (for Any% runs)
            bool StartingAnyPercent = watchers.Map.Current == Maps.Tutorial && !watchers.LoadPause.Current && watchers.LoadPause.Old && watchers.isLoading2.Old;
            if (StartingAnyPercent)
            {
                this.OnStartTrigger?.Invoke(this, StartTrigger.AnyPercent);
                return;
            }

            // Starting Karl's Bay at morning (for Last Loop)
            bool StartingLastLoop = watchers.Map.Current == Maps.KarlsBayMorning && watchers.LoadPause.Old && !watchers.LoadPause.Current && watchers.isLoading2.Old;
            if (StartingLastLoop)
            {
                this.OnStartTrigger?.Invoke(this, StartTrigger.LastLoop);
                return;
            }
        }

        void GameTime()
        {
           // this.OnGameTimeTrigger?.Invoke(this, watchers.LoadPause.Current);

            switch (watchers.Map.Current)
            {
                case Maps.Menu:
                    if (!watchers.LoadPause.Old && watchers.LoadPause.Current)
                    {
                        this.OnGameTimeTrigger?.Invoke(this, true);
                    }
                    if (watchers.isConnectingOnline.Old < 20 && watchers.isConnectingOnline.Current >= 20)
                    {
                        this.OnGameTimeTrigger?.Invoke(this, false);
                    }
                    break;
                default:
                    this.OnGameTimeTrigger?.Invoke(this, watchers.LoadPause.Current);
                    break;
            }
        }

        void Split()
        {
            // Enables splitting when leaving a map and returning to the menu
            if (watchers.Map.Current == Maps.Menu && watchers.Map.Old != watchers.Map.Current && watchers.Map.Old != Maps.InvalidMap)
            {
                this.OnSplitTrigger?.Invoke(this, SplitTrigger.MapLeave);
                return;
            }

            // Enables splitting when reaching the Rakyetoplan and leaving for the final confrontation against Julianna
            if (watchers.Map.Current == Maps.AntennaRak && watchers.Map.Old != watchers.Map.Current && watchers.Map.Old != Maps.InvalidMap)
            {
                this.OnSplitTrigger?.Invoke(this, SplitTrigger.MapAntenna);
                return;
            }

            // Enables split when jumping off the platform at the end of the game
            if (!watchers.LoadPause.Current && watchers.Map.Current == Maps.AntennaRak && watchers.yPos.Current > -66f && watchers.zPos.Current <= 185.7f && watchers.zPos.Old > 185.7f)
            {
                this.OnSplitTrigger?.Invoke(this, SplitTrigger.MapVoid);
                return;
            }
        }

        internal void ResetColtProgression(object sender, EventArgs type)
        {
            try { watchers.ResetColtProgression(game); } catch { }
        }


        internal enum StartTrigger
        {
            AnyPercent,
            LastLoop
        }

        internal enum SplitTrigger
        {
            MapLeave,
            MapAntenna,
            MapVoid
        }

        bool HookGameProcess()
        {
            foreach (string process in new string[] { "Deathloop" })
            {
                game = Process.GetProcessesByName(process).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
                if (game == null) continue;
                try { watchers = new Watchers(game); } catch { game = null; return false; }
                return true;
            }
            return false;
        }
    }
}
