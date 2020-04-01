namespace sweepnet
{
    partial class Editor
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
            this.label1 = new System.Windows.Forms.Label();
            this.text = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.go_button = new System.Windows.Forms.Button();
            this.review = new System.Windows.Forms.CheckBox();
            this.savebutton = new System.Windows.Forms.Button();
            this.closebutton = new System.Windows.Forms.Button();
            this.comm = new System.Windows.Forms.TextBox();
            this.minor = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Line";
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text.Location = new System.Drawing.Point(0, 28);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(575, 289);
            this.text.TabIndex = 1;
            this.text.Text = "";
            this.text.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.text_LinkClicked);
            this.text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_KeyDown);
            this.text.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.text_PreviewKeyDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(117, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(88, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // go_button
            // 
            this.go_button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.go_button.Location = new System.Drawing.Point(211, 5);
            this.go_button.Name = "go_button";
            this.go_button.Size = new System.Drawing.Size(94, 20);
            this.go_button.TabIndex = 3;
            this.go_button.Text = "Go";
            this.go_button.UseVisualStyleBackColor = true;
            this.go_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // review
            // 
            this.review.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.review.AutoSize = true;
            this.review.Checked = true;
            this.review.CheckState = System.Windows.Forms.CheckState.Checked;
            this.review.Location = new System.Drawing.Point(109, 348);
            this.review.Name = "review";
            this.review.Size = new System.Drawing.Size(62, 17);
            this.review.TabIndex = 4;
            this.review.Text = "Review";
            this.review.UseVisualStyleBackColor = true;
            // 
            // savebutton
            // 
            this.savebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.savebutton.Location = new System.Drawing.Point(305, 343);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(125, 25);
            this.savebutton.TabIndex = 5;
            this.savebutton.Text = "Save";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // closebutton
            // 
            this.closebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closebutton.Location = new System.Drawing.Point(436, 343);
            this.closebutton.Name = "closebutton";
            this.closebutton.Size = new System.Drawing.Size(125, 25);
            this.closebutton.TabIndex = 6;
            this.closebutton.Text = "Close";
            this.closebutton.UseVisualStyleBackColor = true;
            this.closebutton.Click += new System.EventHandler(this.button3_Click);
            // 
            // comm
            // 
            this.comm.Location = new System.Drawing.Point(0, 317);
            this.comm.Name = "comm";
            this.comm.Size = new System.Drawing.Size(575, 20);
            this.comm.TabIndex = 7;
            // 
            // minor
            // 
            this.minor.AutoSize = true;
            this.minor.Location = new System.Drawing.Point(13, 348);
            this.minor.Name = "minor";
            this.minor.Size = new System.Drawing.Size(58, 17);
            this.minor.TabIndex = 8;
            this.minor.Text = "Minor?";
            this.minor.UseVisualStyleBackColor = true;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 370);
            this.Controls.Add(this.minor);
            this.Controls.Add(this.comm);
            this.Controls.Add(this.closebutton);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.review);
            this.Controls.Add(this.go_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.text);
            this.Controls.Add(this.label1);
            this.Name = "Editor";
            this.Text = "Editor";
            this.Load += new System.EventHandler(this.Editor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox text;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button go_button;
        private System.Windows.Forms.CheckBox review;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button closebutton;
        private System.Windows.Forms.TextBox comm;
        private System.Windows.Forms.CheckBox minor;
    }
}