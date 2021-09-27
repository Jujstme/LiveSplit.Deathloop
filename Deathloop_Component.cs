using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Deathloop
{
    partial class Component : LogicComponent
    {
        public GameVariables vars = new GameVariables();
        public override string ComponentName => vars.GameName;
        public Process game;
        public TimerModel _timer;
        private System.Windows.Forms.Timer _update_timer;
        public Settings settings { get; set; }

        public Component(LiveSplitState state)
        {
            _timer = new TimerModel { CurrentState = state };
            _update_timer = new System.Windows.Forms.Timer() { Interval = 1000 / vars.refreshRate, Enabled = true };
            settings = new Settings(state);
            _update_timer.Tick += updateLogic;
        }
        public override void Dispose()
        {
            settings.Dispose();
            _update_timer?.Dispose();
        }
        void updateLogic(object sender, EventArgs eventArgs)
        {
            if (game == null || game.HasExited)
            {
                if (!this.TryGetGameProcess()) return;
            }
            update();
            if (_timer.CurrentState.CurrentPhase == TimerPhase.NotRunning) start();
            if (_timer.CurrentState.CurrentPhase == TimerPhase.Running)
            {
                isLoading();
                gameTime();
                resetLogic();
                splitLogic();
            }
        }
        public override XmlNode GetSettings(XmlDocument document) { return this.settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.settings; }

        public override void SetSettings(XmlNode settings) { this.settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }

        private bool TryGetGameProcess()
        {
            foreach (var process in vars.ExeName)
            {
                game = Process.GetProcessesByName(process).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
                if (game != null) break;
            }
            if (game == null) return false;

            // game = Process.GetProcessesByName(vars.ExeName).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
            // game = Process.GetProcessesByName(vars.ExeName).FirstOrDefault(p => !p.HasExited);

            Thread.Sleep(500);
            Init();
            return true;
        }
    }
}
