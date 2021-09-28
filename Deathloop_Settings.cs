using LiveSplit.Model;
using System;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Deathloop
{
    public partial class Settings : UserControl
    {
        public bool runStart { get; set; }
        public bool enableSplitting { get; set; }
        public bool runStartLastLoop { get; set; }
        public bool MapLeave { get; set; }
        public bool MapAntenna { get; set; }
        public bool MapVoid { get; set; }

        private LiveSplitState _state;

        public Settings(LiveSplitState state)
        {
            InitializeComponent();
            _state = state;
            this.Load += Settings_OnLoad;

            // General settings
            this.chkrunStart.DataBindings.Add("Checked", this, "runStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEnableSplitting.DataBindings.Add("Checked", this, "enableSplitting", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkrunStartLastLoop.DataBindings.Add("Checked", this, "runStartLastLoop", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMapLeave.DataBindings.Add("Checked", this, "MapLeave", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMapAntenna.DataBindings.Add("Checked", this, "MapAntenna", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkMapVoid.DataBindings.Add("Checked", this, "MapVoid", false, DataSourceUpdateMode.OnPropertyChanged);

            //
            // Default Values
            //
            this.runStart = true;
            this.enableSplitting = true;
            this.runStartLastLoop = false;
            this.MapLeave = this.MapAntenna = this.MapVoid = true;

            // For settings check
            this.chkMapAntenna.CheckedChanged += Settings_OnLoad;
            this.chkMapLeave.CheckedChanged += Settings_OnLoad;
            this.chkEnableSplitting.CheckedChanged += CheckGraySplitCheckboxes_e;
            this.chkrunStart.CheckedChanged += CheckGraySplitCheckboxes_e;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("settings");
            settingsNode.AppendChild(ToElement(doc, "runStart", this.runStart));
            settingsNode.AppendChild(ToElement(doc, "enableSplitting", this.enableSplitting));
            settingsNode.AppendChild(ToElement(doc, "runStartLastLoop", this.runStartLastLoop));
            settingsNode.AppendChild(ToElement(doc, "MapLeave", this.MapLeave));
            settingsNode.AppendChild(ToElement(doc, "MapAntenna", this.MapAntenna));
            settingsNode.AppendChild(ToElement(doc, "MapVoid", this.MapVoid));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.runStart = ParseBool(settings, "runStart", true);
            this.enableSplitting = ParseBool(settings, "enableSplitting", true);
            this.runStartLastLoop = ParseBool(settings, "runStartLastLoop", false);
            this.MapLeave = ParseBool(settings, "MapLeave", true);
            this.MapAntenna = ParseBool(settings, "MapAntenna", true);
            this.MapVoid = ParseBool(settings, "MapVoid", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        private void Settings_OnLoad(object sender, EventArgs e)
        {
            CheckNumberAutoSplits();
            CheckGraySplitCheckboxes();
        }

        private void CheckNumberAutoSplits()
        {
            int checkedCount = (chkMapLeave.Checked ? 10 : 0) + (chkMapAntenna.Checked ? 1 : 0);
            label4.Text = checkedCount.ToString();
            label3.Text = _state.Run.Count.ToString();

            if (checkedCount != _state.Run.Count)
            {
                this.label3.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
            }
            else
            {
                this.label3.ForeColor = System.Drawing.Color.FromName("ControlText");
            }
        }

        private void chkAnyPercentSplits_Click(object sender, EventArgs e)
        {
            var question = MessageBox.Show("This will set up your splits according to your selected autosplitting options.\n" +
                            "WARNING: Any existing PB recorded for the current layout will be deleted.\n\n" +
                            "Do you want to continue?", "Livesplit - DEATHLOOP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.No) return;
            if (!this.chkMapLeave.Checked && !this.chkMapAntenna.Checked)
            {
                MessageBox.Show("Your selected settings do not include any split.", "Livesplit - DEATHLOOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.chkrunStartLastLoop.Checked = false;
            _state.Run.CategoryName = "Any%";
            _state.Run.Clear();
            if (this.chkMapLeave.Checked)
            {
                _state.Run.AddSegment("Intro");
                _state.Run.AddSegment("Updaam LPP");
                _state.Run.AddSegment("Complex");
                _state.Run.AddSegment("Night Updaam");
                _state.Run.AddSegment("Updaam 2-BIT");
                _state.Run.AddSegment("Egor Code");
                _state.Run.AddSegment("Harriet");
                _state.Run.AddSegment("Noon Complex");
                _state.Run.AddSegment("Fristad Rock");
            }
            if (this.chkMapAntenna.Checked)
            {
                _state.Run.AddSegment("Updaam party");
                _state.Run.AddSegment("Julianna");
            }
            else
            {
                _state.Run.AddSegment("Deathlööp");
            }
            CheckNumberAutoSplits();
        }

        private void CheckGraySplitCheckboxes_e(object sender, EventArgs e) { CheckGraySplitCheckboxes(); }

        private void CheckGraySplitCheckboxes()
        {
            chkMapAntenna.Enabled = chkMapLeave.Enabled = chkMapVoid.Enabled = this.chkEnableSplitting.Checked;
            chkrunStartLastLoop.Enabled = this.chkrunStart.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var question = MessageBox.Show("This will set up your splits according to your selected autosplitting options.\n" +
                            "WARNING: Any existing PB recorded for the current layout will be deleted.\n\n" +
                            "Do you want to continue?", "Livesplit - DEATHLOOP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.No) return;
            if (!this.chkMapLeave.Checked && !this.chkMapAntenna.Checked)
            {
                MessageBox.Show("Your selected settings do not include any split.", "Livesplit - DEATHLOOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.chkrunStartLastLoop.Checked = true;
            _state.Run.CategoryName = "Final Loop";
            _state.Run.Clear();
            if (this.chkMapLeave.Checked)
            {
                _state.Run.AddSegment("Harriet");
                _state.Run.AddSegment("Noon Complex");
                _state.Run.AddSegment("Fristad Rock");
            }
            if (this.chkMapAntenna.Checked)
            {
                _state.Run.AddSegment("Updaam party");
                _state.Run.AddSegment("Julianna");
            }
            else
            {
                _state.Run.AddSegment("Deathlööp");
            }
            CheckNumberAutoSplits();
        }
    }
}
