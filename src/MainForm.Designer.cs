namespace sweepnet
{
    partial class MainForm
    {

        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //stope = 1;
                components.Dispose();
            }
          //  mdsp=1;
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows



        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        /// 
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.traffic_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.main_panel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.control_box = new System.Windows.Forms.Panel();
            this.reload = new System.Windows.Forms.Button();
            this.time_label = new System.Windows.Forms.Label();
            this.pause = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.revbox = new sweepnet.MyRevBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.diff_browser = new System.Windows.Forms.WebBrowser();
            this.panel6 = new System.Windows.Forms.Panel();
            this.listbox = new sweepnet.MyListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.main_menu = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.горячиеКлавишиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверитьОбновленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pages_tool_panel = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.back_article_button = new System.Windows.Forms.ToolStripButton();
            this.review_button = new System.Windows.Forms.ToolStripButton();
            this.next_article_button = new System.Windows.Forms.ToolStripButton();
            this.cancel_button = new System.Windows.Forms.ToolStripButton();
            this.reject_button = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.existence_button = new System.Windows.Forms.ToolStripButton();
            this.show_user_info_button = new System.Windows.Forms.ToolStripButton();
            this.d_diff = new System.Windows.Forms.ToolStripButton();
            this.d_view = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.edit_page_button = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pro_mode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.button_show_stable_version_in_browser = new System.Windows.Forms.ToolStripButton();
            this.button_show_in_browser = new System.Windows.Forms.ToolStripButton();
            this.diff_button = new System.Windows.Forms.ToolStripButton();
            this.show_history_in_browser = new System.Windows.Forms.ToolStripButton();
            this.edit_inside_wikipedia = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stream_config_button = new System.Windows.Forms.ToolStripButton();
            this.restart_stream_button = new System.Windows.Forms.ToolStripButton();
            this.cb_l = new System.Windows.Forms.CheckBox();
            this.revs_tool_panel = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.p_back = new System.Windows.Forms.ToolStripButton();
            this.p_review = new System.Windows.Forms.ToolStripButton();
            this.p_next = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.p_gmc = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.rev_context = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restore_old_v_and_review = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.main_panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.control_box.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.main_menu.SuspendLayout();
            this.pages_tool_panel.SuspendLayout();
            this.revs_tool_panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.rev_context.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.traffic_label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 717);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1276, 26);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(418, 20);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = " ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(418, 20);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 20);
            // 
            // traffic_label
            // 
            this.traffic_label.Name = "traffic_label";
            this.traffic_label.Size = new System.Drawing.Size(418, 20);
            this.traffic_label.Spring = true;
            this.traffic_label.Text = "0 / 0";
            this.traffic_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // main_panel
            // 
            this.main_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.main_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.main_panel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.main_panel.Controls.Add(this.panel2);
            this.main_panel.Controls.Add(this.splitContainer1);
            this.main_panel.Location = new System.Drawing.Point(0, 95);
            this.main_panel.Margin = new System.Windows.Forms.Padding(4);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(1276, 649);
            this.main_panel.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.control_box);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1276, 32);
            this.panel2.TabIndex = 25;
            // 
            // control_box
            // 
            this.control_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.control_box.BackColor = System.Drawing.Color.LightGray;
            this.control_box.Controls.Add(this.reload);
            this.control_box.Controls.Add(this.time_label);
            this.control_box.Controls.Add(this.pause);
            this.control_box.Controls.Add(this.start);
            this.control_box.Location = new System.Drawing.Point(1008, 2);
            this.control_box.Margin = new System.Windows.Forms.Padding(4);
            this.control_box.Name = "control_box";
            this.control_box.Size = new System.Drawing.Size(264, 28);
            this.control_box.TabIndex = 19;
            this.control_box.Visible = false;
            // 
            // reload
            // 
            this.reload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reload.Image = ((System.Drawing.Image)(resources.GetObject("reload.Image")));
            this.reload.Location = new System.Drawing.Point(33, 2);
            this.reload.Margin = new System.Windows.Forms.Padding(0);
            this.reload.Name = "reload";
            this.reload.Size = new System.Drawing.Size(33, 25);
            this.reload.TabIndex = 3;
            this.reload.UseVisualStyleBackColor = true;
            this.reload.Click += new System.EventHandler(this.reload_Click);
            // 
            // time_label
            // 
            this.time_label.AutoSize = true;
            this.time_label.Location = new System.Drawing.Point(116, 6);
            this.time_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.time_label.Name = "time_label";
            this.time_label.Size = new System.Drawing.Size(44, 17);
            this.time_label.TabIndex = 2;
            this.time_label.Text = "00:00";
            // 
            // pause
            // 
            this.pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pause.Image = ((System.Drawing.Image)(resources.GetObject("pause.Image")));
            this.pause.Location = new System.Drawing.Point(67, 2);
            this.pause.Margin = new System.Windows.Forms.Padding(0);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(33, 25);
            this.pause.TabIndex = 1;
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.pause_Click);
            // 
            // start
            // 
            this.start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start.Image = ((System.Drawing.Image)(resources.GetObject("start.Image")));
            this.start.Location = new System.Drawing.Point(0, 2);
            this.start.Margin = new System.Windows.Forms.Padding(0);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(33, 25);
            this.start.TabIndex = 0;
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(56, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(409, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1276, 649);
            this.splitContainer1.SplitterDistance = 328;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.revbox);
            this.panel3.Location = new System.Drawing.Point(0, 34);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(320, 587);
            this.panel3.TabIndex = 0;
            // 
            // revbox
            // 
            this.revbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.revbox.FormattingEnabled = true;
            this.revbox.ItemHeight = 18;
            this.revbox.Location = new System.Drawing.Point(0, 0);
            this.revbox.Margin = new System.Windows.Forms.Padding(4);
            this.revbox.Name = "revbox";
            this.revbox.Size = new System.Drawing.Size(320, 587);
            this.revbox.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel6);
            this.splitContainer2.Size = new System.Drawing.Size(943, 649);
            this.splitContainer2.SplitterDistance = 734;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.diff_browser);
            this.panel5.Location = new System.Drawing.Point(4, 17);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(726, 636);
            this.panel5.TabIndex = 1;
            // 
            // diff_browser
            // 
            this.diff_browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diff_browser.IsWebBrowserContextMenuEnabled = false;
            this.diff_browser.Location = new System.Drawing.Point(0, 0);
            this.diff_browser.Margin = new System.Windows.Forms.Padding(4);
            this.diff_browser.MinimumSize = new System.Drawing.Size(27, 25);
            this.diff_browser.Name = "diff_browser";
            this.diff_browser.ScriptErrorsSuppressed = true;
            this.diff_browser.Size = new System.Drawing.Size(726, 636);
            this.diff_browser.TabIndex = 27;
            this.diff_browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.diff_browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.listbox);
            this.panel6.Location = new System.Drawing.Point(7, 33);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(191, 615);
            this.panel6.TabIndex = 0;
            // 
            // listbox
            // 
            this.listbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listbox.FormattingEnabled = true;
            this.listbox.ItemHeight = 18;
            this.listbox.Location = new System.Drawing.Point(0, 0);
            this.listbox.Margin = new System.Windows.Forms.Padding(4);
            this.listbox.Name = "listbox";
            this.listbox.Size = new System.Drawing.Size(191, 615);
            this.listbox.TabIndex = 2;
            this.listbox.SelectedIndexChanged += new System.EventHandler(this.listbox_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 333;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 5000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // main_menu
            // 
            this.main_menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.main_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.горячиеКлавишиToolStripMenuItem,
            this.проверитьОбновленияToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.main_menu.Location = new System.Drawing.Point(0, 0);
            this.main_menu.Name = "main_menu";
            this.main_menu.Size = new System.Drawing.Size(1276, 28);
            this.main_menu.TabIndex = 17;
            this.main_menu.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.настройкиToolStripMenuItem.Text = "Options";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // горячиеКлавишиToolStripMenuItem
            // 
            this.горячиеКлавишиToolStripMenuItem.Name = "горячиеКлавишиToolStripMenuItem";
            this.горячиеКлавишиToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.горячиеКлавишиToolStripMenuItem.Text = "Hot Keys";
            this.горячиеКлавишиToolStripMenuItem.Click += new System.EventHandler(this.горячиеКлавишиToolStripMenuItem_Click);
            // 
            // проверитьОбновленияToolStripMenuItem
            // 
            this.проверитьОбновленияToolStripMenuItem.Name = "проверитьОбновленияToolStripMenuItem";
            this.проверитьОбновленияToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.проверитьОбновленияToolStripMenuItem.Text = "Check updates";
            this.проверитьОбновленияToolStripMenuItem.Click += new System.EventHandler(this.CheckUpdatesToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.оПрограммеToolStripMenuItem.Text = "About";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(946, 0);
            this.toolStrip3.TabIndex = 0;
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 0);
            this.toolStripButton6.Text = "toolStripButton6";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(-5, -8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(951, 10);
            this.panel4.TabIndex = 6;
            // 
            // pages_tool_panel
            // 
            this.pages_tool_panel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.pages_tool_panel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.back_article_button,
            this.review_button,
            this.next_article_button,
            this.cancel_button,
            this.reject_button,
            this.toolStripSeparator4,
            this.existence_button,
            this.show_user_info_button,
            this.d_diff,
            this.d_view,
            this.toolStripSeparator1,
            this.edit_page_button,
            this.toolStripSeparator3,
            this.pro_mode,
            this.toolStripSeparator5,
            this.button_show_stable_version_in_browser,
            this.button_show_in_browser,
            this.diff_button,
            this.show_history_in_browser,
            this.edit_inside_wikipedia,
            this.toolStripSeparator2,
            this.stream_config_button,
            this.restart_stream_button});
            this.pages_tool_panel.Location = new System.Drawing.Point(0, 28);
            this.pages_tool_panel.Name = "pages_tool_panel";
            this.pages_tool_panel.Size = new System.Drawing.Size(1276, 27);
            this.pages_tool_panel.TabIndex = 18;
            this.pages_tool_panel.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(58, 24);
            this.toolStripLabel1.Text = "Articles";
            // 
            // back_article_button
            // 
            this.back_article_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.back_article_button.Image = ((System.Drawing.Image)(resources.GetObject("back_article_button.Image")));
            this.back_article_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.back_article_button.Name = "back_article_button";
            this.back_article_button.Size = new System.Drawing.Size(29, 24);
            this.back_article_button.Text = "Back";
            this.back_article_button.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // review_button
            // 
            this.review_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.review_button.Image = ((System.Drawing.Image)(resources.GetObject("review_button.Image")));
            this.review_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.review_button.Name = "review_button";
            this.review_button.Size = new System.Drawing.Size(29, 24);
            this.review_button.Text = "Review";
            this.review_button.Click += new System.EventHandler(this.review_button_Click);
            // 
            // next_article_button
            // 
            this.next_article_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.next_article_button.Image = ((System.Drawing.Image)(resources.GetObject("next_article_button.Image")));
            this.next_article_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.next_article_button.Name = "next_article_button";
            this.next_article_button.Size = new System.Drawing.Size(29, 24);
            this.next_article_button.Text = "Skip";
            this.next_article_button.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cancel_button.Image = ((System.Drawing.Image)(resources.GetObject("cancel_button.Image")));
            this.cancel_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(29, 24);
            this.cancel_button.Text = "Cancel (with custom summary)";
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // reject_button
            // 
            this.reject_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.reject_button.Image = ((System.Drawing.Image)(resources.GetObject("reject_button.Image")));
            this.reject_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reject_button.Name = "reject_button";
            this.reject_button.Size = new System.Drawing.Size(29, 24);
            this.reject_button.Text = "Revert changes";
            this.reject_button.Click += new System.EventHandler(this.reject_button_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // existence_button
            // 
            this.existence_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.existence_button.Image = ((System.Drawing.Image)(resources.GetObject("existence_button.Image")));
            this.existence_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.existence_button.Name = "existence_button";
            this.existence_button.Size = new System.Drawing.Size(29, 24);
            this.existence_button.Text = "Check the existence of a file/page (or select and press s)";
            this.existence_button.Click += new System.EventHandler(this.existence_button_Click);
            // 
            // show_user_info_button
            // 
            this.show_user_info_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.show_user_info_button.Image = ((System.Drawing.Image)(resources.GetObject("show_user_info_button.Image")));
            this.show_user_info_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.show_user_info_button.Name = "show_user_info_button";
            this.show_user_info_button.Size = new System.Drawing.Size(29, 24);
            this.show_user_info_button.Text = "Показать информацию об участниках (F,f)";
            this.show_user_info_button.Click += new System.EventHandler(this.show_user_info_button_Click);
            // 
            // d_diff
            // 
            this.d_diff.Checked = true;
            this.d_diff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.d_diff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.d_diff.Image = ((System.Drawing.Image)(resources.GetObject("d_diff.Image")));
            this.d_diff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.d_diff.Name = "d_diff";
            this.d_diff.Size = new System.Drawing.Size(29, 24);
            this.d_diff.Text = "Show source code (difference)";
            this.d_diff.Click += new System.EventHandler(this.d_diff_Click);
            // 
            // d_view
            // 
            this.d_view.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.d_view.Image = ((System.Drawing.Image)(resources.GetObject("d_view.Image")));
            this.d_view.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.d_view.Name = "d_view";
            this.d_view.Size = new System.Drawing.Size(29, 24);
            this.d_view.Text = "Show parsed page";
            this.d_view.Click += new System.EventHandler(this.d_view_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // edit_page_button
            // 
            this.edit_page_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edit_page_button.Image = ((System.Drawing.Image)(resources.GetObject("edit_page_button.Image")));
            this.edit_page_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_page_button.Name = "edit_page_button";
            this.edit_page_button.Size = new System.Drawing.Size(29, 24);
            this.edit_page_button.Text = "Edit page";
            this.edit_page_button.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // pro_mode
            // 
            this.pro_mode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pro_mode.Enabled = false;
            this.pro_mode.Image = ((System.Drawing.Image)(resources.GetObject("pro_mode.Image")));
            this.pro_mode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pro_mode.Name = "pro_mode";
            this.pro_mode.Size = new System.Drawing.Size(29, 24);
            this.pro_mode.Text = "toolStripButton4";
            this.pro_mode.Click += new System.EventHandler(this.pro_mode_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // button_show_stable_version_in_browser
            // 
            this.button_show_stable_version_in_browser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_show_stable_version_in_browser.Image = ((System.Drawing.Image)(resources.GetObject("button_show_stable_version_in_browser.Image")));
            this.button_show_stable_version_in_browser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_show_stable_version_in_browser.Name = "button_show_stable_version_in_browser";
            this.button_show_stable_version_in_browser.Size = new System.Drawing.Size(29, 24);
            this.button_show_stable_version_in_browser.Text = "Pass stable version to your browser";
            this.button_show_stable_version_in_browser.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // button_show_in_browser
            // 
            this.button_show_in_browser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_show_in_browser.Image = ((System.Drawing.Image)(resources.GetObject("button_show_in_browser.Image")));
            this.button_show_in_browser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_show_in_browser.Name = "button_show_in_browser";
            this.button_show_in_browser.Size = new System.Drawing.Size(29, 24);
            this.button_show_in_browser.Text = "Pass current version to your browser";
            this.button_show_in_browser.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // diff_button
            // 
            this.diff_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diff_button.Image = ((System.Drawing.Image)(resources.GetObject("diff_button.Image")));
            this.diff_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diff_button.Name = "diff_button";
            this.diff_button.Size = new System.Drawing.Size(29, 24);
            this.diff_button.Text = "Pass the difference page to your browser";
            this.diff_button.Click += new System.EventHandler(this.diff_button_Click);
            // 
            // show_history_in_browser
            // 
            this.show_history_in_browser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.show_history_in_browser.Image = ((System.Drawing.Image)(resources.GetObject("show_history_in_browser.Image")));
            this.show_history_in_browser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.show_history_in_browser.Name = "show_history_in_browser";
            this.show_history_in_browser.Size = new System.Drawing.Size(29, 24);
            this.show_history_in_browser.Text = "Open this page\'s history in your browser";
            this.show_history_in_browser.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // edit_inside_wikipedia
            // 
            this.edit_inside_wikipedia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edit_inside_wikipedia.Image = ((System.Drawing.Image)(resources.GetObject("edit_inside_wikipedia.Image")));
            this.edit_inside_wikipedia.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_inside_wikipedia.Name = "edit_inside_wikipedia";
            this.edit_inside_wikipedia.Size = new System.Drawing.Size(29, 24);
            this.edit_inside_wikipedia.Text = "Edit this page in your browser";
            this.edit_inside_wikipedia.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // stream_config_button
            // 
            this.stream_config_button.AutoToolTip = false;
            this.stream_config_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stream_config_button.Image = ((System.Drawing.Image)(resources.GetObject("stream_config_button.Image")));
            this.stream_config_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stream_config_button.Name = "stream_config_button";
            this.stream_config_button.Size = new System.Drawing.Size(153, 24);
            this.stream_config_button.Text = "Stream configuration";
            this.stream_config_button.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // restart_stream_button
            // 
            this.restart_stream_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.restart_stream_button.Image = ((System.Drawing.Image)(resources.GetObject("restart_stream_button.Image")));
            this.restart_stream_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.restart_stream_button.Name = "restart_stream_button";
            this.restart_stream_button.Size = new System.Drawing.Size(29, 24);
            this.restart_stream_button.Text = "Restart the stream";
            this.restart_stream_button.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // cb_l
            // 
            this.cb_l.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_l.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_l.Image = ((System.Drawing.Image)(resources.GetObject("cb_l.Image")));
            this.cb_l.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cb_l.Location = new System.Drawing.Point(3, 4);
            this.cb_l.Margin = new System.Windows.Forms.Padding(0);
            this.cb_l.Name = "cb_l";
            this.cb_l.Size = new System.Drawing.Size(39, 30);
            this.cb_l.TabIndex = 25;
            this.cb_l.UseVisualStyleBackColor = true;
            this.cb_l.CheckedChanged += new System.EventHandler(this.cb_l_CheckedChanged);
            // 
            // revs_tool_panel
            // 
            this.revs_tool_panel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.revs_tool_panel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.p_back,
            this.p_review,
            this.p_next,
            this.toolStripSeparator6,
            this.p_gmc});
            this.revs_tool_panel.Location = new System.Drawing.Point(0, 55);
            this.revs_tool_panel.Name = "revs_tool_panel";
            this.revs_tool_panel.Size = new System.Drawing.Size(1276, 27);
            this.revs_tool_panel.TabIndex = 26;
            this.revs_tool_panel.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(58, 24);
            this.toolStripLabel2.Text = "Revs:    ";
            // 
            // p_back
            // 
            this.p_back.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.p_back.Image = ((System.Drawing.Image)(resources.GetObject("p_back.Image")));
            this.p_back.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.p_back.Name = "p_back";
            this.p_back.Size = new System.Drawing.Size(29, 24);
            this.p_back.Text = "To the previous difference";
            this.p_back.Click += new System.EventHandler(this.p_back_Click);
            // 
            // p_review
            // 
            this.p_review.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.p_review.Image = ((System.Drawing.Image)(resources.GetObject("p_review.Image")));
            this.p_review.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.p_review.Name = "p_review";
            this.p_review.Size = new System.Drawing.Size(29, 24);
            this.p_review.Text = "Review the selected changes";
            this.p_review.Click += new System.EventHandler(this.p_review_Click);
            // 
            // p_next
            // 
            this.p_next.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.p_next.Image = ((System.Drawing.Image)(resources.GetObject("p_next.Image")));
            this.p_next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.p_next.Name = "p_next";
            this.p_next.Size = new System.Drawing.Size(29, 24);
            this.p_next.Text = "To the following difference";
            this.p_next.Click += new System.EventHandler(this.p_next_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // p_gmc
            // 
            this.p_gmc.Checked = true;
            this.p_gmc.CheckOnClick = true;
            this.p_gmc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.p_gmc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.p_gmc.Image = ((System.Drawing.Image)(resources.GetObject("p_gmc.Image")));
            this.p_gmc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.p_gmc.Name = "p_gmc";
            this.p_gmc.Size = new System.Drawing.Size(29, 24);
            this.p_gmc.Text = "Take grouping into account";
            this.p_gmc.Click += new System.EventHandler(this.p_gmc_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cb_l);
            this.panel1.Location = new System.Drawing.Point(1231, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(41, 34);
            this.panel1.TabIndex = 4;
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 333;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // rev_context
            // 
            this.rev_context.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.rev_context.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.restore_old_v_and_review,
            this.toolStripMenuItem2,
            this.backToolStripMenuItem});
            this.rev_context.Name = "rev_context";
            this.rev_context.Size = new System.Drawing.Size(300, 106);
            this.rev_context.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.rev_context_Closed);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(299, 24);
            this.sourceToolStripMenuItem.Text = "Source text";
            this.sourceToolStripMenuItem.Click += new System.EventHandler(this.sourceToolStripMenuItem_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(299, 24);
            this.ViewToolStripMenuItem.Text = "Parsed revision";
            this.ViewToolStripMenuItem.Click += new System.EventHandler(this.ViewToolStripMenuItem_Click);
            // 
            // restore_old_v_and_review
            // 
            this.restore_old_v_and_review.Name = "restore_old_v_and_review";
            this.restore_old_v_and_review.Size = new System.Drawing.Size(299, 24);
            this.restore_old_v_and_review.Text = "Restore this revision and review it";
            this.restore_old_v_and_review.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(296, 6);
            // 
            // backToolStripMenuItem
            // 
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(299, 24);
            this.backToolStripMenuItem.Text = "Back";
            this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1276, 743);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.revs_tool_panel);
            this.Controls.Add(this.pages_tool_panel);
            this.Controls.Add(this.main_menu);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.main_panel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.main_panel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.control_box.ResumeLayout(false);
            this.control_box.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.main_menu.ResumeLayout(false);
            this.main_menu.PerformLayout();
            this.pages_tool_panel.ResumeLayout(false);
            this.pages_tool_panel.PerformLayout();
            this.revs_tool_panel.ResumeLayout(false);
            this.revs_tool_panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.rev_context.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Panel main_panel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel control_box;
        private System.Windows.Forms.Button reload;
        private System.Windows.Forms.Label time_label;
        private System.Windows.Forms.Button pause;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip main_menu;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверитьОбновленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ToolStrip pages_tool_panel;
        private System.Windows.Forms.ToolStripButton back_article_button;
        private System.Windows.Forms.ToolStripButton review_button;
        private System.Windows.Forms.ToolStripButton next_article_button;
        private System.Windows.Forms.ToolStripButton cancel_button;
        private System.Windows.Forms.ToolStripButton reject_button;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton d_diff;
        private System.Windows.Forms.ToolStripButton d_view;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton edit_page_button;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton button_show_stable_version_in_browser;
        private System.Windows.Forms.ToolStripButton button_show_in_browser;
        private System.Windows.Forms.ToolStripButton diff_button;
        private System.Windows.Forms.ToolStripButton show_history_in_browser;
        private System.Windows.Forms.ToolStripButton edit_inside_wikipedia;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton stream_config_button;
        private System.Windows.Forms.ToolStripButton restart_stream_button;
        private System.Windows.Forms.CheckBox cb_l;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStrip revs_tool_panel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton p_back;
        private System.Windows.Forms.ToolStripButton p_review;
        private System.Windows.Forms.ToolStripButton p_next;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
      //  private MyRevBox revbox;
        private System.Windows.Forms.SplitContainer splitContainer2;
      //  private MyListBox listbox;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.WebBrowser diff_browser;
        private System.Windows.Forms.Panel panel6;
        private MyListBox listbox;
        private System.Windows.Forms.ToolStripButton pro_mode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.ContextMenuStrip rev_context;
        private System.Windows.Forms.ToolStripMenuItem sourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton p_gmc;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel traffic_label;
        private MyRevBox revbox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripMenuItem restore_old_v_and_review;
        private System.Windows.Forms.ToolStripMenuItem горячиеКлавишиToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton show_user_info_button;
        private System.Windows.Forms.ToolStripButton existence_button;
       // private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

