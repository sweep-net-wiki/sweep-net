namespace sweepnet
{
    partial class AuthForm
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
            this.invitation_label = new System.Windows.Forms.Label();
            this.authorize_button = new System.Windows.Forms.Button();
            this.password_textbox = new System.Windows.Forms.TextBox();
            this.login_textbox = new System.Windows.Forms.TextBox();
            this.project_combo = new System.Windows.Forms.ComboBox();
            this.version_label = new System.Windows.Forms.Label();
            this.save_pass = new System.Windows.Forms.CheckBox();
            this.langbox = new System.Windows.Forms.ComboBox();
            this.language_label = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // invitation_label
            // 
            this.invitation_label.AutoSize = true;
            this.invitation_label.Location = new System.Drawing.Point(59, 11);
            this.invitation_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.invitation_label.Name = "invitation_label";
            this.invitation_label.Size = new System.Drawing.Size(200, 17);
            this.invitation_label.TabIndex = 3;
            this.invitation_label.Text = "Enter your login and password";
            // 
            // authorize_button
            // 
            this.authorize_button.Location = new System.Drawing.Point(15, 209);
            this.authorize_button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.authorize_button.Name = "authorize_button";
            this.authorize_button.Size = new System.Drawing.Size(324, 33);
            this.authorize_button.TabIndex = 2;
            this.authorize_button.Text = "Authorize";
            this.authorize_button.UseVisualStyleBackColor = true;
            this.authorize_button.Click += new System.EventHandler(this.authorize_button_click);
            // 
            // password_textbox
            // 
            this.password_textbox.Location = new System.Drawing.Point(17, 66);
            this.password_textbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.password_textbox.Name = "password_textbox";
            this.password_textbox.PasswordChar = '*';
            this.password_textbox.Size = new System.Drawing.Size(320, 22);
            this.password_textbox.TabIndex = 1;
            this.password_textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // login_textbox
            // 
            this.login_textbox.Location = new System.Drawing.Point(16, 34);
            this.login_textbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.login_textbox.Name = "login_textbox";
            this.login_textbox.Size = new System.Drawing.Size(321, 22);
            this.login_textbox.TabIndex = 0;
            // 
            // project_combo
            // 
            this.project_combo.BackColor = System.Drawing.Color.White;
            this.project_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.project_combo.FormattingEnabled = true;
            this.project_combo.Location = new System.Drawing.Point(16, 98);
            this.project_combo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.project_combo.Name = "project_combo";
            this.project_combo.Size = new System.Drawing.Size(273, 24);
            this.project_combo.TabIndex = 4;
            this.project_combo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // version_label
            // 
            this.version_label.AutoSize = true;
            this.version_label.Location = new System.Drawing.Point(3, 246);
            this.version_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.version_label.Name = "version_label";
            this.version_label.Size = new System.Drawing.Size(46, 17);
            this.version_label.TabIndex = 5;
            this.version_label.Text = "label2";
            // 
            // save_pass
            // 
            this.save_pass.AutoSize = true;
            this.save_pass.Location = new System.Drawing.Point(16, 181);
            this.save_pass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.save_pass.Name = "save_pass";
            this.save_pass.Size = new System.Drawing.Size(163, 21);
            this.save_pass.TabIndex = 6;
            this.save_pass.Text = "Remember password";
            this.save_pass.UseVisualStyleBackColor = true;
            // 
            // langbox
            // 
            this.langbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langbox.FormattingEnabled = true;
            this.langbox.Location = new System.Drawing.Point(17, 148);
            this.langbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.langbox.Name = "langbox";
            this.langbox.Size = new System.Drawing.Size(320, 24);
            this.langbox.TabIndex = 7;
            this.langbox.SelectedIndexChanged += new System.EventHandler(this.langbox_SelectedIndexChanged);
            // 
            // language_label
            // 
            this.language_label.AutoSize = true;
            this.language_label.Location = new System.Drawing.Point(13, 128);
            this.language_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.language_label.Name = "language_label";
            this.language_label.Size = new System.Drawing.Size(122, 17);
            this.language_label.TabIndex = 8;
            this.language_label.Text = "Language / Язык:";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(299, 98);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 26);
            this.button2.TabIndex = 9;
            this.button2.Text = "+";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Auth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(353, 263);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.language_label);
            this.Controls.Add(this.langbox);
            this.Controls.Add(this.save_pass);
            this.Controls.Add(this.version_label);
            this.Controls.Add(this.project_combo);
            this.Controls.Add(this.invitation_label);
            this.Controls.Add(this.authorize_button);
            this.Controls.Add(this.password_textbox);
            this.Controls.Add(this.login_textbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Auth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authorization";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Auth_FormClosed);
            this.Load += new System.EventHandler(this.Auth_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button authorize_button;
        public System.Windows.Forms.TextBox password_textbox;
        public System.Windows.Forms.TextBox login_textbox;
        private System.Windows.Forms.Label invitation_label;
        private System.Windows.Forms.ComboBox project_combo;
        private System.Windows.Forms.Label version_label;
        private System.Windows.Forms.CheckBox save_pass;
        private System.Windows.Forms.ComboBox langbox;
        private System.Windows.Forms.Label language_label;
        private System.Windows.Forms.Button button2;
    }
}