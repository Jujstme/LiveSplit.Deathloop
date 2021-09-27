namespace LiveSplit.Deathloop
{
    partial class Settings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkDisableOffset = new System.Windows.Forms.CheckBox();
            this.chkEnableSplitting = new System.Windows.Forms.CheckBox();
            this.chkrunStart = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMapVoid = new System.Windows.Forms.CheckBox();
            this.chkMapAntenna = new System.Windows.Forms.CheckBox();
            this.chkMapLeave = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAnyPercentSplits = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDisableOffset);
            this.groupBox1.Controls.Add(this.chkEnableSplitting);
            this.groupBox1.Controls.Add(this.chkrunStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(455, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Options";
            // 
            // chkDisableOffset
            // 
            this.chkDisableOffset.AutoSize = true;
            this.chkDisableOffset.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkDisableOffset.Location = new System.Drawing.Point(166, 23);
            this.chkDisableOffset.Name = "chkDisableOffset";
            this.chkDisableOffset.Size = new System.Drawing.Size(115, 17);
            this.chkDisableOffset.TabIndex = 3;
            this.chkDisableOffset.Text = "Disable timer offset";
            this.toolTip1.SetToolTip(this.chkDisableOffset, "Enabling this options sets the timer offsets to zero (default: -51.5 seconds).\r\nU" +
        "se this when you want the timer to start at 0 seconds (eg. Final Loop category)." +
        "");
            this.chkDisableOffset.UseVisualStyleBackColor = true;
            // 
            // chkEnableSplitting
            // 
            this.chkEnableSplitting.AutoSize = true;
            this.chkEnableSplitting.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkEnableSplitting.Location = new System.Drawing.Point(83, 23);
            this.chkEnableSplitting.Name = "chkEnableSplitting";
            this.chkEnableSplitting.Size = new System.Drawing.Size(83, 17);
            this.chkEnableSplitting.TabIndex = 2;
            this.chkEnableSplitting.Text = "Autosplitting";
            this.chkEnableSplitting.UseVisualStyleBackColor = true;
            // 
            // chkrunStart
            // 
            this.chkrunStart.AutoSize = true;
            this.chkrunStart.Checked = true;
            this.chkrunStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkrunStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkrunStart.Location = new System.Drawing.Point(10, 23);
            this.chkrunStart.Name = "chkrunStart";
            this.chkrunStart.Size = new System.Drawing.Size(73, 17);
            this.chkrunStart.TabIndex = 0;
            this.chkrunStart.Text = "Auto Start";
            this.chkrunStart.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.chkMapVoid);
            this.groupBox2.Controls.Add(this.chkMapAntenna);
            this.groupBox2.Controls.Add(this.chkMapLeave);
            this.groupBox2.Location = new System.Drawing.Point(10, 70);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(455, 84);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Autosplitting";
            // 
            // chkMapVoid
            // 
            this.chkMapVoid.AutoSize = true;
            this.chkMapVoid.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkMapVoid.Location = new System.Drawing.Point(10, 57);
            this.chkMapVoid.Name = "chkMapVoid";
            this.chkMapVoid.Size = new System.Drawing.Size(435, 17);
            this.chkMapVoid.TabIndex = 2;
            this.chkMapVoid.Text = "Autosplit when jumping into the void at the game end";
            this.chkMapVoid.UseVisualStyleBackColor = true;
            // 
            // chkMapAntenna
            // 
            this.chkMapAntenna.AutoSize = true;
            this.chkMapAntenna.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkMapAntenna.Location = new System.Drawing.Point(10, 40);
            this.chkMapAntenna.Name = "chkMapAntenna";
            this.chkMapAntenna.Size = new System.Drawing.Size(435, 17);
            this.chkMapAntenna.TabIndex = 1;
            this.chkMapAntenna.Text = "Autosplit when reaching the antenna on last loop";
            this.chkMapAntenna.UseVisualStyleBackColor = true;
            // 
            // chkMapLeave
            // 
            this.chkMapLeave.AutoSize = true;
            this.chkMapLeave.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkMapLeave.Location = new System.Drawing.Point(10, 23);
            this.chkMapLeave.Name = "chkMapLeave";
            this.chkMapLeave.Size = new System.Drawing.Size(435, 17);
            this.chkMapLeave.TabIndex = 0;
            this.chkMapLeave.Text = "Autosplit when leaving a map";
            this.chkMapLeave.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.chkAnyPercentSplits);
            this.groupBox3.Location = new System.Drawing.Point(10, 164);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.groupBox3.Size = new System.Drawing.Size(455, 65);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto Setup";
            // 
            // chkAnyPercentSplits
            // 
            this.chkAnyPercentSplits.Location = new System.Drawing.Point(13, 26);
            this.chkAnyPercentSplits.Name = "chkAnyPercentSplits";
            this.chkAnyPercentSplits.Size = new System.Drawing.Size(127, 23);
            this.chkAnyPercentSplits.TabIndex = 3;
            this.chkAnyPercentSplits.Text = "Any%";
            this.chkAnyPercentSplits.UseVisualStyleBackColor = true;
            this.chkAnyPercentSplits.Click += new System.EventHandler(this.chkAnyPercentSplits_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(10, 242);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(455, 80);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Information";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(381, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(200, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of splits in the current layout:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total numer of splits required for Any% run according to the current settings:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Last Loop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(475, 500);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkrunStart;
        private System.Windows.Forms.CheckBox chkEnableSplitting;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkMapVoid;
        private System.Windows.Forms.CheckBox chkMapAntenna;
        private System.Windows.Forms.CheckBox chkMapLeave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button chkAnyPercentSplits;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDisableOffset;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
    }
}
