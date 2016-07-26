namespace Antnf.KeyboardRemote.Client.Receiver
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.OutputTextBox = new System.Windows.Forms.TextBox();
			this.TrayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyKeyDown = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyKeyUp = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.changeAddrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.TrayNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayNotifyIcon.Icon")));
			this.TrayNotifyIcon.Text = "Keyboard Remote";
			this.TrayNotifyIcon.Visible = true;
			// 
			// TrayMenu
			// 
			this.TrayMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.stateToolStripMenuItem,
            this.enableToolStripMenuItem,
            this.notifyToolStripMenuItem,
            this.toolStripSeparator1,
            this.changeAddrToolStripMenuItem,
            this.showConsoleToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.TrayMenu.Name = "TrayMenu";
			this.TrayMenu.ShowCheckMargin = true;
			this.TrayMenu.ShowImageMargin = false;
			this.TrayMenu.Size = new System.Drawing.Size(173, 164);
			// 
			// connectToolStripMenuItem
			// 
			this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
			this.connectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.connectToolStripMenuItem.Text = "&Connect";
			this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
			// 
			// stateToolStripMenuItem
			// 
			this.stateToolStripMenuItem.Enabled = false;
			this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
			this.stateToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.stateToolStripMenuItem.Text = "Offline";
			// 
			// enableToolStripMenuItem
			// 
			this.enableToolStripMenuItem.CheckOnClick = true;
			this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
			this.enableToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.enableToolStripMenuItem.Text = "Enable";
			// 
			// notifyToolStripMenuItem
			// 
			this.notifyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotifyConnection,
            this.NotifyKeyDown,
            this.NotifyKeyUp});
			this.notifyToolStripMenuItem.Name = "notifyToolStripMenuItem";
			this.notifyToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.notifyToolStripMenuItem.Text = "&Notify";
			// 
			// NotifyConnection
			// 
			this.NotifyConnection.Checked = true;
			this.NotifyConnection.CheckOnClick = true;
			this.NotifyConnection.CheckState = System.Windows.Forms.CheckState.Checked;
			this.NotifyConnection.Name = "NotifyConnection";
			this.NotifyConnection.Size = new System.Drawing.Size(141, 22);
			this.NotifyConnection.Text = "Connection";
			// 
			// NotifyKeyDown
			// 
			this.NotifyKeyDown.CheckOnClick = true;
			this.NotifyKeyDown.Name = "NotifyKeyDown";
			this.NotifyKeyDown.Size = new System.Drawing.Size(141, 22);
			this.NotifyKeyDown.Text = "KeyDown";
			// 
			// NotifyKeyUp
			// 
			this.NotifyKeyUp.CheckOnClick = true;
			this.NotifyKeyUp.Name = "NotifyKeyUp";
			this.NotifyKeyUp.Size = new System.Drawing.Size(141, 22);
			this.NotifyKeyUp.Text = "KeyUp";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
			// 
			// changeAddrToolStripMenuItem
			// 
			this.changeAddrToolStripMenuItem.Name = "changeAddrToolStripMenuItem";
			this.changeAddrToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.changeAddrToolStripMenuItem.Text = "Change &Address";
			this.changeAddrToolStripMenuItem.Click += new System.EventHandler(this.changeAddrToolStripMenuItem_Click);
			// 
			// showConsoleToolStripMenuItem
			// 
			this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
			this.showConsoleToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.showConsoleToolStripMenuItem.Text = "&Show Console";
			this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
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
        private System.Windows.Forms.ToolStripMenuItem NotifyConnection;
        private System.Windows.Forms.ToolStripMenuItem NotifyKeyDown;
        private System.Windows.Forms.ToolStripMenuItem NotifyKeyUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem notifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeAddrToolStripMenuItem;
    }
}

