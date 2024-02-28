namespace ConfigTools
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExcelPath = new System.Windows.Forms.TextBox();
            this.SelectExcelPath = new System.Windows.Forms.Button();
            this.ScriptableObjectButton = new System.Windows.Forms.Button();
            this.BinaryButton = new System.Windows.Forms.Button();
            this.BuildProgressBar = new System.Windows.Forms.ProgressBar();
            this.LogPanel = new System.Windows.Forms.Panel();
            this.LogLabel = new System.Windows.Forms.Label();
            this.OutputPath = new System.Windows.Forms.TextBox();
            this.OutputButton = new System.Windows.Forms.Button();
            this.ExcelPathError = new System.Windows.Forms.LinkLabel();
            this.OutputPathError = new System.Windows.Forms.LinkLabel();
            this.ClearLog = new System.Windows.Forms.Button();
            this.UseLanguage = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BuildSign = new System.Windows.Forms.ComboBox();
            this.LogPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExcelPath
            // 
            this.ExcelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcelPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExcelPath.Location = new System.Drawing.Point(30, 32);
            this.ExcelPath.Name = "ExcelPath";
            this.ExcelPath.Size = new System.Drawing.Size(517, 24);
            this.ExcelPath.TabIndex = 0;
            this.ExcelPath.TextChanged += new System.EventHandler(this.ExcelPath_TextChanged);
            // 
            // SelectExcelPath
            // 
            this.SelectExcelPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectExcelPath.Location = new System.Drawing.Point(580, 30);
            this.SelectExcelPath.Name = "SelectExcelPath";
            this.SelectExcelPath.Size = new System.Drawing.Size(120, 30);
            this.SelectExcelPath.TabIndex = 1;
            this.SelectExcelPath.Text = "选择excel路径";
            this.SelectExcelPath.UseVisualStyleBackColor = true;
            this.SelectExcelPath.Click += new System.EventHandler(this.SelectExcelPath_Click);
            // 
            // ScriptableObjectButton
            // 
            this.ScriptableObjectButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScriptableObjectButton.Location = new System.Drawing.Point(30, 152);
            this.ScriptableObjectButton.Name = "ScriptableObjectButton";
            this.ScriptableObjectButton.Size = new System.Drawing.Size(145, 35);
            this.ScriptableObjectButton.TabIndex = 2;
            this.ScriptableObjectButton.Text = "导出ScriptableObject";
            this.ScriptableObjectButton.UseVisualStyleBackColor = true;
            this.ScriptableObjectButton.Click += new System.EventHandler(this.ScriptableObjectButton_Click);
            // 
            // BinaryButton
            // 
            this.BinaryButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BinaryButton.Location = new System.Drawing.Point(220, 152);
            this.BinaryButton.Name = "BinaryButton";
            this.BinaryButton.Size = new System.Drawing.Size(200, 35);
            this.BinaryButton.TabIndex = 3;
            this.BinaryButton.Text = "导出二进制";
            this.BinaryButton.UseVisualStyleBackColor = true;
            this.BinaryButton.Click += new System.EventHandler(this.BinaryButton_Click);
            // 
            // BuildProgressBar
            // 
            this.BuildProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BuildProgressBar.Location = new System.Drawing.Point(30, 215);
            this.BuildProgressBar.Name = "BuildProgressBar";
            this.BuildProgressBar.Size = new System.Drawing.Size(660, 30);
            this.BuildProgressBar.TabIndex = 4;
            // 
            // LogPanel
            // 
            this.LogPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogPanel.Controls.Add(this.LogLabel);
            this.LogPanel.Location = new System.Drawing.Point(30, 272);
            this.LogPanel.Name = "LogPanel";
            this.LogPanel.Size = new System.Drawing.Size(660, 160);
            this.LogPanel.TabIndex = 5;
            this.LogPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.LogPanel_Paint);
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(28, 12);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(44, 17);
            this.LogLabel.TabIndex = 0;
            this.LogLabel.Text = "日志：";
            // 
            // OutputPath
            // 
            this.OutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OutputPath.Location = new System.Drawing.Point(30, 93);
            this.OutputPath.Name = "OutputPath";
            this.OutputPath.Size = new System.Drawing.Size(517, 24);
            this.OutputPath.TabIndex = 6;
            this.OutputPath.TextChanged += new System.EventHandler(this.OutputPath_TextChanged);
            // 
            // OutputButton
            // 
            this.OutputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputButton.Location = new System.Drawing.Point(582, 91);
            this.OutputButton.Name = "OutputButton";
            this.OutputButton.Size = new System.Drawing.Size(120, 30);
            this.OutputButton.TabIndex = 7;
            this.OutputButton.Text = "导出路径";
            this.OutputButton.UseVisualStyleBackColor = true;
            this.OutputButton.Click += new System.EventHandler(this.OutputButton_Click);
            // 
            // ExcelPathError
            // 
            this.ExcelPathError.AutoSize = true;
            this.ExcelPathError.LinkColor = System.Drawing.Color.Red;
            this.ExcelPathError.Location = new System.Drawing.Point(30, 59);
            this.ExcelPathError.Name = "ExcelPathError";
            this.ExcelPathError.Size = new System.Drawing.Size(56, 17);
            this.ExcelPathError.TabIndex = 8;
            this.ExcelPathError.TabStop = true;
            this.ExcelPathError.Text = "路径错误";
            // 
            // OutputPathError
            // 
            this.OutputPathError.AutoSize = true;
            this.OutputPathError.LinkColor = System.Drawing.Color.Red;
            this.OutputPathError.Location = new System.Drawing.Point(30, 120);
            this.OutputPathError.Name = "OutputPathError";
            this.OutputPathError.Size = new System.Drawing.Size(56, 17);
            this.OutputPathError.TabIndex = 9;
            this.OutputPathError.TabStop = true;
            this.OutputPathError.Text = "路径错误";
            // 
            // ClearLog
            // 
            this.ClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearLog.Location = new System.Drawing.Point(615, 438);
            this.ClearLog.Name = "ClearLog";
            this.ClearLog.Size = new System.Drawing.Size(75, 25);
            this.ClearLog.TabIndex = 10;
            this.ClearLog.Text = "Clear";
            this.ClearLog.UseVisualStyleBackColor = true;
            this.ClearLog.Click += new System.EventHandler(this.ClearLog_Click);
            // 
            // UseLanguage
            // 
            this.UseLanguage.AutoSize = true;
            this.UseLanguage.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.UseLanguage.Location = new System.Drawing.Point(451, 158);
            this.UseLanguage.Name = "UseLanguage";
            this.UseLanguage.Size = new System.Drawing.Size(93, 26);
            this.UseLanguage.TabIndex = 11;
            this.UseLanguage.Text = "是否翻译";
            this.UseLanguage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(607, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "导出配置标记";
            // 
            // BuildSign
            // 
            this.BuildSign.FormattingEnabled = true;
            this.BuildSign.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.BuildSign.Location = new System.Drawing.Point(569, 158);
            this.BuildSign.Name = "BuildSign";
            this.BuildSign.Size = new System.Drawing.Size(32, 25);
            this.BuildSign.TabIndex = 14;
            this.BuildSign.Text = "0";
            this.BuildSign.SelectedIndexChanged += new System.EventHandler(this.BuildSign_SelectedIndexChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 473);
            this.Controls.Add(this.BuildSign);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UseLanguage);
            this.Controls.Add(this.ClearLog);
            this.Controls.Add(this.OutputPathError);
            this.Controls.Add(this.ExcelPathError);
            this.Controls.Add(this.OutputButton);
            this.Controls.Add(this.OutputPath);
            this.Controls.Add(this.LogPanel);
            this.Controls.Add(this.BuildProgressBar);
            this.Controls.Add(this.BinaryButton);
            this.Controls.Add(this.ScriptableObjectButton);
            this.Controls.Add(this.SelectExcelPath);
            this.Controls.Add(this.ExcelPath);
            this.Name = "MainWindow";
            this.Text = "窗口";
            this.LogPanel.ResumeLayout(false);
            this.LogPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox ExcelPath;
        private Button SelectExcelPath;
        private Button ScriptableObjectButton;
        private Button BinaryButton;
        private ProgressBar BuildProgressBar;
        private Panel LogPanel;
        private Label LogLabel;
        private TextBox OutputPath;
        private Button OutputButton;
        private LinkLabel ExcelPathError;
        private LinkLabel OutputPathError;
        private Button ClearLog;
        private CheckBox UseLanguage;
        private Label label1;
        private ComboBox BuildSign;
    }
}