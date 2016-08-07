namespace KeyboardRemote.Client.Receiver
{
    partial class Main
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
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.TrayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changeAddrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useEmbeddedServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputTextBox.Location = new System.Drawing.Point(12, 12);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputTextBox.Size = new System.Drawing.Size(713, 709);
            this.OutputTextBox.TabIndex = 2;
            this.OutputTextBox.WordWrap = false;
            // 
            // TrayNotifyIcon
            // 
            this.TrayNotifyIcon.ContextMenuStrip = this.TrayMenu;
            this.TrayNotifyIcon.Text = "Receiver [Disconnect]";
            this.TrayNotifyIcon.Visible = true;
            // 
            // TrayMenu
            // 
            this.TrayMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.enableToolStripMenuItem,
            this.notifyToolStripMenuItem,
            this.toolStripSeparator1,
            this.changeAddrToolStripMenuItem,
            this.useEmbeddedServerToolStripMenuItem,
            this.toolStripSeparator2,
            this.showConsoleToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.TrayMenu.Name = "TrayMenu";
            this.TrayMenu.ShowCheckMargin = true;
            this.TrayMenu.ShowImageMargin = false;
            this.TrayMenu.Size = new System.Drawing.Size(273, 259);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Checked = true;
            this.enableToolStripMenuItem.CheckOnClick = true;
            this.enableToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.enableToolStripMenuItem.Text = "Enable";
            // 
            // notifyToolStripMenuItem
            // 
            this.notifyToolStripMenuItem.CheckOnClick = true;
            this.notifyToolStripMenuItem.Name = "notifyToolStripMenuItem";
            this.notifyToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.notifyToolStripMenuItem.Text = "&Notify";
            this.notifyToolStripMenuItem.Click += new System.EventHandler(this.notifyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(269, 6);
            // 
            // changeAddrToolStripMenuItem
            // 
            this.changeAddrToolStripMenuItem.Name = "changeAddrToolStripMenuItem";
            this.changeAddrToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.changeAddrToolStripMenuItem.Text = "Change &Address";
            this.changeAddrToolStripMenuItem.Click += new System.EventHandler(this.changeAddrToolStripMenuItem_Click);
            // 
            // useEmbeddedServerToolStripMenuItem
            // 
            this.useEmbeddedServerToolStripMenuItem.Name = "useEmbeddedServerToolStripMenuItem";
            this.useEmbeddedServerToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.useEmbeddedServerToolStripMenuItem.Text = "Use Embedded Server";
            this.useEmbeddedServerToolStripMenuItem.Click += new System.EventHandler(this.useEmbeddedServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(269, 6);
            // 
            // showConsoleToolStripMenuItem
            // 
            this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
            this.showConsoleToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.showConsoleToolStripMenuItem.Text = "&Show Console";
            this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(737, 733);
            this.Controls.Add(this.OutputTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keyboard Remote Client";
            this.Load += new System.EventHandler(this.Main_Load);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.NotifyIcon TrayNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem notifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeAddrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useEmbeddedServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

