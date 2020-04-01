namespace sweepnet
{
    partial class SettingsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_common = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.auto_scroll = new System.Windows.Forms.CheckBox();
            this.diff_style = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.check_links = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.recent_count = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.check_updates = new System.Windows.Forms.CheckBox();
            this.tabPage_detailed = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buff3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buff2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buff1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.applyonlytotheproject2 = new System.Windows.Forms.CheckBox();
            this.applyonlytotheproject1 = new System.Windows.Forms.CheckBox();
            this.rj_comm = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.rw_comm = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.skip_null = new System.Windows.Forms.CheckBox();
            this.tabPage_pro = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.pd_wait = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.closebutton = new System.Windows.Forms.Button();
            this.def = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage_common.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage_detailed.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_pro.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_common);
            this.tabControl1.Controls.Add(this.tabPage_detailed);
            this.tabControl1.Controls.Add(this.tabPage_pro);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(494, 354);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_common
            // 
            this.tabPage_common.Controls.Add(this.groupBox5);
            this.tabPage_common.Controls.Add(this.groupBox4);
            this.tabPage_common.Controls.Add(this.groupBox2);
            this.tabPage_common.Location = new System.Drawing.Point(4, 22);
            this.tabPage_common.Name = "tabPage_common";
            this.tabPage_common.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_common.Size = new System.Drawing.Size(486, 328);
            this.tabPage_common.TabIndex = 0;
            this.tabPage_common.Text = "Common";
            this.tabPage_common.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.auto_scroll);
            this.groupBox5.Controls.Add(this.diff_style);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.check_links);
            this.groupBox5.Location = new System.Drawing.Point(8, 126);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(470, 113);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Pages";
            // 
            // auto_scroll
            // 
            this.auto_scroll.AutoSize = true;
            this.auto_scroll.Location = new System.Drawing.Point(17, 75);
            this.auto_scroll.Name = "auto_scroll";
            this.auto_scroll.Size = new System.Drawing.Size(247, 17);
            this.auto_scroll.TabIndex = 6;
            this.auto_scroll.Text = "Authomatically scroll page to the first difference";
            this.auto_scroll.UseVisualStyleBackColor = true;
            // 
            // diff_style
            // 
            this.diff_style.BackColor = System.Drawing.Color.White;
            this.diff_style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diff_style.FormattingEnabled = true;
            this.diff_style.Items.AddRange(new object[] {
            "Новое",
            "Старое"});
            this.diff_style.Location = new System.Drawing.Point(207, 46);
            this.diff_style.Name = "diff_style";
            this.diff_style.Size = new System.Drawing.Size(206, 21);
            this.diff_style.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 49);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Diff style";
            // 
            // check_links
            // 
            this.check_links.AutoSize = true;
            this.check_links.Location = new System.Drawing.Point(17, 19);
            this.check_links.Name = "check_links";
            this.check_links.Size = new System.Drawing.Size(254, 17);
            this.check_links.TabIndex = 0;
            this.check_links.Text = "Grant the ability to check the existence of pages";
            this.check_links.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.recent_count);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(8, 62);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(470, 58);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Real-time changes";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(321, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "recent edits";
            // 
            // recent_count
            // 
            this.recent_count.Location = new System.Drawing.Point(224, 24);
            this.recent_count.Name = "recent_count";
            this.recent_count.Size = new System.Drawing.Size(91, 20);
            this.recent_count.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Control the state of ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.check_updates);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 50);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Updates";
            // 
            // check_updates
            // 
            this.check_updates.AutoSize = true;
            this.check_updates.Location = new System.Drawing.Point(17, 19);
            this.check_updates.Name = "check_updates";
            this.check_updates.Size = new System.Drawing.Size(162, 17);
            this.check_updates.TabIndex = 0;
            this.check_updates.Text = "Check updates every launch";
            this.check_updates.UseVisualStyleBackColor = true;
            // 
            // tabPage_detailed
            // 
            this.tabPage_detailed.Controls.Add(this.groupBox3);
            this.tabPage_detailed.Controls.Add(this.groupBox1);
            this.tabPage_detailed.Location = new System.Drawing.Point(4, 22);
            this.tabPage_detailed.Name = "tabPage_detailed";
            this.tabPage_detailed.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_detailed.Size = new System.Drawing.Size(486, 328);
            this.tabPage_detailed.TabIndex = 1;
            this.tabPage_detailed.Text = "Detailed";
            this.tabPage_detailed.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.buff3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.buff2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.buff1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(6, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(472, 91);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Buffers";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(345, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "articles (3-480)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "articles (5-480)";
            // 
            // buff3
            // 
            this.buff3.Location = new System.Drawing.Point(232, 64);
            this.buff3.Name = "buff3";
            this.buff3.Size = new System.Drawing.Size(87, 20);
            this.buff3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Preload differences for";
            // 
            // buff2
            // 
            this.buff2.Location = new System.Drawing.Point(232, 40);
            this.buff2.Name = "buff2";
            this.buff2.Size = new System.Drawing.Size(87, 20);
            this.buff2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "In real-time changes mode preload for";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(345, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "articles (5-480)";
            // 
            // buff1
            // 
            this.buff1.Location = new System.Drawing.Point(232, 17);
            this.buff1.Name = "buff1";
            this.buff1.Size = new System.Drawing.Size(87, 20);
            this.buff1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Preload data for";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.applyonlytotheproject2);
            this.groupBox1.Controls.Add(this.applyonlytotheproject1);
            this.groupBox1.Controls.Add(this.rj_comm);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.rw_comm);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.skip_null);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 218);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reviewing";
            // 
            // applyonlytotheproject2
            // 
            this.applyonlytotheproject2.AutoSize = true;
            this.applyonlytotheproject2.Location = new System.Drawing.Point(17, 195);
            this.applyonlytotheproject2.Name = "applyonlytotheproject2";
            this.applyonlytotheproject2.Size = new System.Drawing.Size(140, 17);
            this.applyonlytotheproject2.TabIndex = 6;
            this.applyonlytotheproject2.Text = "Apply only to this project";
            this.applyonlytotheproject2.UseVisualStyleBackColor = true;
            // 
            // applyonlytotheproject1
            // 
            this.applyonlytotheproject1.AutoSize = true;
            this.applyonlytotheproject1.Location = new System.Drawing.Point(17, 100);
            this.applyonlytotheproject1.Name = "applyonlytotheproject1";
            this.applyonlytotheproject1.Size = new System.Drawing.Size(140, 17);
            this.applyonlytotheproject1.TabIndex = 5;
            this.applyonlytotheproject1.Text = "Apply only to this project";
            this.applyonlytotheproject1.UseVisualStyleBackColor = true;
            // 
            // rj_comm
            // 
            this.rj_comm.Location = new System.Drawing.Point(17, 170);
            this.rj_comm.Name = "rj_comm";
            this.rj_comm.Size = new System.Drawing.Size(435, 20);
            this.rj_comm.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 128);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(323, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Custom reject summary (similarly; {0} stands for the revision number)";
            // 
            // rw_comm
            // 
            this.rw_comm.Location = new System.Drawing.Point(17, 73);
            this.rw_comm.Name = "rw_comm";
            this.rw_comm.Size = new System.Drawing.Size(435, 20);
            this.rw_comm.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(346, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Custom review summary (leave empty if you want to use the default one)";
            // 
            // skip_null
            // 
            this.skip_null.AutoSize = true;
            this.skip_null.Location = new System.Drawing.Point(17, 19);
            this.skip_null.Name = "skip_null";
            this.skip_null.Size = new System.Drawing.Size(344, 17);
            this.skip_null.TabIndex = 0;
            this.skip_null.Text = "Skip articles with no difference between current and stable versions";
            this.skip_null.UseVisualStyleBackColor = true;
            // 
            // tabPage_pro
            // 
            this.tabPage_pro.Controls.Add(this.groupBox6);
            this.tabPage_pro.Location = new System.Drawing.Point(4, 22);
            this.tabPage_pro.Name = "tabPage_pro";
            this.tabPage_pro.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_pro.Size = new System.Drawing.Size(486, 328);
            this.tabPage_pro.TabIndex = 2;
            this.tabPage_pro.Text = "Pro mode";
            this.tabPage_pro.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.pd_wait);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(472, 70);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Revision list";
            // 
            // pd_wait
            // 
            this.pd_wait.Location = new System.Drawing.Point(10, 39);
            this.pd_wait.Name = "pd_wait";
            this.pd_wait.Size = new System.Drawing.Size(197, 20);
            this.pd_wait.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(230, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "(ms, 1s = 1000ms)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(288, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Delay between choosing revisions and displaying difference";
            // 
            // closebutton
            // 
            this.closebutton.Location = new System.Drawing.Point(387, 2);
            this.closebutton.Name = "closebutton";
            this.closebutton.Size = new System.Drawing.Size(95, 23);
            this.closebutton.TabIndex = 2;
            this.closebutton.Text = "Close";
            this.closebutton.UseVisualStyleBackColor = true;
            this.closebutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // def
            // 
            this.def.Location = new System.Drawing.Point(140, 2);
            this.def.Name = "def";
            this.def.Size = new System.Drawing.Size(135, 23);
            this.def.TabIndex = 1;
            this.def.Text = "Default";
            this.def.UseVisualStyleBackColor = true;
            this.def.Click += new System.EventHandler(this.def_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(281, 2);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(100, 23);
            this.save.TabIndex = 0;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.closebutton);
            this.panel1.Controls.Add(this.def);
            this.panel1.Controls.Add(this.save);
            this.panel1.Location = new System.Drawing.Point(0, 349);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 28);
            this.panel1.TabIndex = 1;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 378);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_common.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage_detailed.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_pro.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_common;
        private System.Windows.Forms.TabPage tabPage_detailed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox buff3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox buff2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox buff1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox skip_null;
        private System.Windows.Forms.Button def;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button closebutton;
        private System.Windows.Forms.CheckBox check_updates;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox recent_count;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox check_links;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox rw_comm;
        private System.Windows.Forms.TabPage tabPage_pro;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox pd_wait;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox rj_comm;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox diff_style;
        private System.Windows.Forms.CheckBox auto_scroll;
        private System.Windows.Forms.CheckBox applyonlytotheproject2;
        private System.Windows.Forms.CheckBox applyonlytotheproject1;
    }
}