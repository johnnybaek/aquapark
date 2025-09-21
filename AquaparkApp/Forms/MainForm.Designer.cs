namespace AquaparkApp.Forms
{
    partial class MainForm
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
            this._headerPanel = new System.Windows.Forms.Panel();
            this._sidebarPanel = new System.Windows.Forms.Panel();
            this._contentPanel = new System.Windows.Forms.Panel();
            this._welcomeLabel = new System.Windows.Forms.Label();
            this._loginButton = new AquaparkApp.Controls.MacOSButton();
            this._registerButton = new AquaparkApp.Controls.MacOSButton();
            this._logoutButton = new AquaparkApp.Controls.MacOSButton();
            this._attractionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._mainTabControl = new System.Windows.Forms.TabControl();
            this._headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _headerPanel
            // 
            this._headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this._headerPanel.Controls.Add(this._welcomeLabel);
            this._headerPanel.Controls.Add(this._loginButton);
            this._headerPanel.Controls.Add(this._registerButton);
            this._headerPanel.Controls.Add(this._logoutButton);
            this._headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerPanel.Location = new System.Drawing.Point(0, 0);
            this._headerPanel.Name = "_headerPanel";
            this._headerPanel.Size = new System.Drawing.Size(1400, 80);
            this._headerPanel.TabIndex = 0;
            // 
            // _sidebarPanel
            // 
            this._sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._sidebarPanel.Location = new System.Drawing.Point(0, 80);
            this._sidebarPanel.Name = "_sidebarPanel";
            this._sidebarPanel.Size = new System.Drawing.Size(260, 820);
            this._sidebarPanel.TabIndex = 1;
            // 
            // _contentPanel
            // 
            this._contentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._contentPanel.Location = new System.Drawing.Point(260, 80);
            this._contentPanel.Name = "_contentPanel";
            this._contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this._contentPanel.Size = new System.Drawing.Size(1140, 820);
            this._contentPanel.TabIndex = 2;
            // 
            // _welcomeLabel
            // 
            this._welcomeLabel.AutoSize = true;
            this._welcomeLabel.Font = new System.Drawing.Font("SF Pro Text", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._welcomeLabel.ForeColor = System.Drawing.Color.White;
            this._welcomeLabel.Location = new System.Drawing.Point(10, 25);
            this._welcomeLabel.Name = "_welcomeLabel";
            this._welcomeLabel.Size = new System.Drawing.Size(0, 23);
            this._welcomeLabel.TabIndex = 0;
            this._welcomeLabel.Visible = false;
            // 
            // _loginButton
            // 
            this._loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._loginButton.Font = new System.Drawing.Font("SF Pro Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._loginButton.Location = new System.Drawing.Point(1200, 20);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(100, 35);
            this._loginButton.TabIndex = 1;
            this._loginButton.Text = "Войти";
            this._loginButton.UseVisualStyleBackColor = true;
            // 
            // _registerButton
            // 
            this._registerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._registerButton.Font = new System.Drawing.Font("SF Pro Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._registerButton.Location = new System.Drawing.Point(1310, 20);
            this._registerButton.Name = "_registerButton";
            this._registerButton.Size = new System.Drawing.Size(120, 35);
            this._registerButton.TabIndex = 2;
            this._registerButton.Text = "Регистрация";
            this._registerButton.UseVisualStyleBackColor = true;
            // 
            // _logoutButton
            // 
            this._logoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._logoutButton.Font = new System.Drawing.Font("SF Pro Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._logoutButton.Location = new System.Drawing.Point(1200, 20);
            this._logoutButton.Name = "_logoutButton";
            this._logoutButton.Size = new System.Drawing.Size(100, 35);
            this._logoutButton.TabIndex = 3;
            this._logoutButton.Text = "Выйти";
            this._logoutButton.UseVisualStyleBackColor = true;
            this._logoutButton.Visible = false;
            // 
            // _attractionsPanel
            // 
            this._attractionsPanel.AutoScroll = true;
            this._attractionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._attractionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this._attractionsPanel.Location = new System.Drawing.Point(20, 20);
            this._attractionsPanel.Name = "_attractionsPanel";
            this._attractionsPanel.Padding = new System.Windows.Forms.Padding(10);
            this._attractionsPanel.Size = new System.Drawing.Size(1100, 780);
            this._attractionsPanel.TabIndex = 0;
            this._attractionsPanel.WrapContents = true;
            // 
            // _mainTabControl
            // 
            this._mainTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this._mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this._mainTabControl.Font = new System.Drawing.Font("SF Pro Display", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._mainTabControl.ItemSize = new System.Drawing.Size(120, 40);
            this._mainTabControl.Location = new System.Drawing.Point(20, 20);
            this._mainTabControl.Name = "_mainTabControl";
            this._mainTabControl.SelectedIndex = 0;
            this._mainTabControl.Size = new System.Drawing.Size(1100, 780);
            this._mainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this._mainTabControl.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this._contentPanel);
            this.Controls.Add(this._sidebarPanel);
            this.Controls.Add(this._headerPanel);
            this.Font = new System.Drawing.Font("SF Pro Display", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Аквапарк \"Водный мир\" - Система управления";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this._headerPanel.ResumeLayout(false);
            this._headerPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _headerPanel;
        private System.Windows.Forms.Panel _sidebarPanel;
        private System.Windows.Forms.Panel _contentPanel;
        private System.Windows.Forms.Label _welcomeLabel;
        private AquaparkApp.Controls.MacOSButton _loginButton;
        private AquaparkApp.Controls.MacOSButton _registerButton;
        private AquaparkApp.Controls.MacOSButton _logoutButton;
        private System.Windows.Forms.FlowLayoutPanel _attractionsPanel;
        private System.Windows.Forms.TabControl _mainTabControl;
    }
}
