using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Deathloop
{
    class Component : LogicComponent
    {
        public override string ComponentName => "DEATHLOOP Autosplitter";
        private Settings settings { get; set; }
        private readonly TimerModel timer;
        private readonly Timer update_timer;
        private readonly SplittingLogic SplittingLogic;

        public Component(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            settings = new Settings(state);

            SplittingLogic = new SplittingLogic();
            SplittingLogic.OnStartTrigger += OnStartTrigger;
            SplittingLogic.OnSplitTrigger += OnSplitTrigger;
            SplittingLogic.OnGameTimeTrigger += OnGameTimeTrigger;
            settings.ResetColtProgression += SplittingLogic.ResetColtProgression;

            update_timer = new Timer() { Enabled = true, Interval = 15 };
            update_timer.Tick += updateTimer_Tick;
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            update_timer.Enabled = false;
            SplittingLogic.Update();
            update_timer.Enabled = true;
        }

        void OnStartTrigger(object sender, SplittingLogic.StartTrigger type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.NotRunning) return;
            if (!settings.runStart) return;
            switch (type)
            {
                case SplittingLogic.StartTrigger.AnyPercent:
                    timer.CurrentState.Run.Offset = TimeSpan.FromSeconds(-51.5);
                    timer.Start();
                    timer.CurrentState.Run.Offset = TimeSpan.Zero;
                    break;
                case SplittingLogic.StartTrigger.LastLoop:
                    if (!settings.runStartLastLoop) return;
                    timer.CurrentState.Run.Offset = TimeSpan.Zero;
                    timer.Start();
                    break;
            }
        }

        void OnGameTimeTrigger(object sender, bool isGameTimePaused)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            timer.CurrentState.IsGameTimePaused = isGameTimePaused;
        }

        void OnSplitTrigger(object sender, SplittingLogic.SplitTrigger type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            if (!settings.enableSplitting) return;
            switch (type)
            {
                case SplittingLogic.SplitTrigger.MapLeave:
                    if (settings.MapLeave) timer.Split();
                    break;
                case SplittingLogic.SplitTrigger.MapAntenna:
                    if (settings.MapAntenna) timer.Split();
                    break;
                case SplittingLogic.SplitTrigger.MapVoid:
                    if (settings.MapVoid) timer.Split();
                    break;
            }
        }

        public override void Dispose()
        {
            settings.Dispose();
            update_timer.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.settings; }

        public override void SetSettings(XmlNode settings) { this.settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
