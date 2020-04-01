namespace sweepnet
{
    partial class SiteManager
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.apiurl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sitename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.удалитьВыделеннуюСтрокуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savebutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.apiurl,
            this.sitename});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(484, 215);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // apiurl
            // 
            this.apiurl.HeaderText = "URL for API requests (example)";
            this.apiurl.Name = "apiurl";
            this.apiurl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.apiurl.ToolTipText = "http://ru.wikipedia.org/";
            // 
            // sitename
            // 
            this.sitename.HeaderText = "Name (to be shown in the list)";
            this.sitename.Name = "sitename";
            this.sitename.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьВыделеннуюСтрокуToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(232, 26);
            // 
            // удалитьВыделеннуюСтрокуToolStripMenuItem
            // 
            this.удалитьВыделеннуюСтрокуToolStripMenuItem.Name = "удалитьВыделеннуюСтрокуToolStripMenuItem";
            this.удалитьВыделеннуюСтрокуToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.удалитьВыделеннуюСтрокуToolStripMenuItem.Text = "Удалить выделенную строку";
            this.удалитьВыделеннуюСтрокуToolStripMenuItem.Click += new System.EventHandler(this.удалитьВыделеннуюСтрокуToolStripMenuItem_Click);
            // 
            // savebutton
            // 
            this.savebutton.Location = new System.Drawing.Point(375, 215);
            this.savebutton.Margin = new System.Windows.Forms.Padding(0);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(109, 30);
            this.savebutton.TabIndex = 1;
            this.savebutton.Text = "Save";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.Location = new System.Drawing.Point(268, 215);
            this.cancelbutton.Margin = new System.Windows.Forms.Padding(0);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(107, 30);
            this.cancelbutton.TabIndex = 2;
            this.cancelbutton.Text = "Close";
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // SiteManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 243);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SiteManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of projects";
            this.Load += new System.EventHandler(this.SiteManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem удалитьВыделеннуюСтрокуToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn apiurl;
        private System.Windows.Forms.DataGridViewTextBoxColumn sitename;
    }
}