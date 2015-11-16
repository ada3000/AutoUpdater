namespace AK.Service.Test
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.cbConfigFileName = new System.Windows.Forms.ComboBox();
			this.txtXmlConfig = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbStart = new System.Windows.Forms.ToolStripButton();
			this.tsbUpdateConfig = new System.Windows.Forms.ToolStripButton();
			this.label2 = new System.Windows.Forms.Label();
			this.lbLog = new System.Windows.Forms.ListBox();
			this.cbLoadConfig = new System.Windows.Forms.Button();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Файл настроек";
			// 
			// cbConfigFileName
			// 
			this.cbConfigFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbConfigFileName.FormattingEnabled = true;
			this.cbConfigFileName.Items.AddRange(new object[] {
            "Processes.Config.xml",
            "FoF.Storage.Node.Process.xml",
            "proc_cfg\\FoF.State.ServiceProcessInfo.xml",
            "FoF.Search.Node.Process.xml",
            "proc_cfg\\FoF.State.ClusterMonitor.xml",
            "proc_cfg\\FoF.State.ClusterManager.xml",
            "FoF.Storage.Main.Ping.Config.xml",
            "FoF.Storage.Main.Process.Config.xml",
            "FoF.Catalog.Process.Config.Example.xml"});
			this.cbConfigFileName.Location = new System.Drawing.Point(115, 29);
			this.cbConfigFileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.cbConfigFileName.Name = "cbConfigFileName";
			this.cbConfigFileName.Size = new System.Drawing.Size(360, 24);
			this.cbConfigFileName.TabIndex = 1;
			this.cbConfigFileName.SelectedIndexChanged += new System.EventHandler(this.cbConfigFileName_SelectedIndexChanged);
			// 
			// txtXmlConfig
			// 
			this.txtXmlConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtXmlConfig.Location = new System.Drawing.Point(15, 60);
			this.txtXmlConfig.Multiline = true;
			this.txtXmlConfig.Name = "txtXmlConfig";
			this.txtXmlConfig.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtXmlConfig.Size = new System.Drawing.Size(538, 409);
			this.txtXmlConfig.TabIndex = 2;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStart,
            this.tsbUpdateConfig});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(948, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbStart
			// 
			this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbStart.Image")));
			this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbStart.Name = "tsbStart";
			this.tsbStart.Size = new System.Drawing.Size(61, 22);
			this.tsbStart.Text = "Пуск (F5)";
			this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
			// 
			// tsbUpdateConfig
			// 
			this.tsbUpdateConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbUpdateConfig.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdateConfig.Image")));
			this.tsbUpdateConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbUpdateConfig.Name = "tsbUpdateConfig";
			this.tsbUpdateConfig.Size = new System.Drawing.Size(174, 22);
			this.tsbUpdateConfig.Text = "Обновить параметры (Ctrl+S)";
			this.tsbUpdateConfig.Click += new System.EventHandler(this.tsbUpdateConfig_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(578, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Лог";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// lbLog
			// 
			this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbLog.FormattingEnabled = true;
			this.lbLog.ItemHeight = 16;
			this.lbLog.Location = new System.Drawing.Point(581, 60);
			this.lbLog.Name = "lbLog";
			this.lbLog.Size = new System.Drawing.Size(355, 404);
			this.lbLog.TabIndex = 7;
			// 
			// cbLoadConfig
			// 
			this.cbLoadConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbLoadConfig.Location = new System.Drawing.Point(481, 29);
			this.cbLoadConfig.Name = "cbLoadConfig";
			this.cbLoadConfig.Size = new System.Drawing.Size(75, 23);
			this.cbLoadConfig.TabIndex = 8;
			this.cbLoadConfig.Text = "Загрузить";
			this.cbLoadConfig.UseVisualStyleBackColor = true;
			this.cbLoadConfig.Click += new System.EventHandler(this.cbLoadConfig_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(948, 481);
			this.Controls.Add(this.cbLoadConfig);
			this.Controls.Add(this.lbLog);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.txtXmlConfig);
			this.Controls.Add(this.cbConfigFileName);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Form1";
			this.Text = "Тестирование контейнера Srv.Common";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbConfigFileName;
		private System.Windows.Forms.TextBox txtXmlConfig;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbStart;
		private System.Windows.Forms.ToolStripButton tsbUpdateConfig;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbLog;
		private System.Windows.Forms.Button cbLoadConfig;
	}
}

