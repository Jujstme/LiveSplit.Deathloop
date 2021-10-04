using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Deathloop
{
    partial class Component : LogicComponent
    {
        private GameVariables vars = new GameVariables();
        public override string ComponentName => vars.GameName;
        private Process game;
        private TimerModel _timer;
        private Timer _update_timer;
        private Settings settings { get; set; }

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

        private void updateLogic(object sender, EventArgs eventArgs)
        {
            if (game == null || game.HasExited)
            {
                try
                {
                    if (!HookGameProcess()) return;
                }
                catch
                {
                    game = null;
                    return;
                }
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

        private bool HookGameProcess()
        {
            foreach (var process in vars.ExeName)
            {
                game = Process.GetProcessesByName(process).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
                if (game == null) continue;
                if (Init())
                {
                    return true;
                }
                else
                {
                    game = null;
                    return false;
                }
            }
            return false;
        }
    }
}
