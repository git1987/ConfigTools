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
            ExcelPath = new TextBox();
            SelectExcelPath = new Button();
            ScriptableObjectButton = new Button();
            BinaryButton = new Button();
            BuildProgressBar = new ProgressBar();
            LogPanel = new Panel();
            LogLabel = new Label();
            ExcelPathError = new LinkLabel();
            ClearLog = new Button();
            UseLanguage = new CheckBox();
            label1 = new Label();
            BuildSign = new ComboBox();
            LogPanel.SuspendLayout();
            SuspendLayout();
            // 
            // ExcelPath
            // 
            ExcelPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ExcelPath.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ExcelPath.Location = new Point(30, 32);
            ExcelPath.Name = "ExcelPath";
            ExcelPath.Size = new Size(517, 24);
            ExcelPath.TabIndex = 0;
            ExcelPath.TextChanged += ExcelPath_TextChanged;
            // 
            // SelectExcelPath
            // 
            SelectExcelPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SelectExcelPath.Location = new Point(580, 30);
            SelectExcelPath.Name = "SelectExcelPath";
            SelectExcelPath.Size = new Size(120, 30);
            SelectExcelPath.TabIndex = 1;
            SelectExcelPath.Text = "选择excel路径";
            SelectExcelPath.UseVisualStyleBackColor = true;
            SelectExcelPath.Click += SelectExcelPath_Click;
            // 
            // ScriptableObjectButton
            // 
            ScriptableObjectButton.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            ScriptableObjectButton.Location = new Point(30, 100);
            ScriptableObjectButton.Name = "ScriptableObjectButton";
            ScriptableObjectButton.Size = new Size(145, 35);
            ScriptableObjectButton.TabIndex = 2;
            ScriptableObjectButton.Text = "导出ScriptableObject";
            ScriptableObjectButton.UseVisualStyleBackColor = true;
            ScriptableObjectButton.Click += ScriptableObjectButton_Click;
            // 
            // BinaryButton
            // 
            BinaryButton.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            BinaryButton.Location = new Point(228, 100);
            BinaryButton.Name = "BinaryButton";
            BinaryButton.Size = new Size(200, 35);
            BinaryButton.TabIndex = 3;
            BinaryButton.Text = "导出二进制";
            BinaryButton.UseVisualStyleBackColor = true;
            BinaryButton.Click += BinaryButton_Click;
            // 
            // BuildProgressBar
            // 
            BuildProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BuildProgressBar.Location = new Point(30, 215);
            BuildProgressBar.Name = "BuildProgressBar";
            BuildProgressBar.Size = new Size(660, 30);
            BuildProgressBar.TabIndex = 4;
            // 
            // LogPanel
            // 
            LogPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LogPanel.Controls.Add(LogLabel);
            LogPanel.Location = new Point(30, 272);
            LogPanel.Name = "LogPanel";
            LogPanel.Size = new Size(660, 160);
            LogPanel.TabIndex = 5;
            LogPanel.Paint += LogPanel_Paint;
            // 
            // LogLabel
            // 
            LogLabel.AutoSize = true;
            LogLabel.Location = new Point(28, 12);
            LogLabel.Name = "LogLabel";
            LogLabel.Size = new Size(44, 17);
            LogLabel.TabIndex = 0;
            LogLabel.Text = "日志：";
            // 
            // ExcelPathError
            // 
            ExcelPathError.AutoSize = true;
            ExcelPathError.LinkColor = Color.Red;
            ExcelPathError.Location = new Point(30, 59);
            ExcelPathError.Name = "ExcelPathError";
            ExcelPathError.Size = new Size(56, 17);
            ExcelPathError.TabIndex = 8;
            ExcelPathError.TabStop = true;
            ExcelPathError.Text = "路径错误";
            // 
            // ClearLog
            // 
            ClearLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ClearLog.Location = new Point(615, 438);
            ClearLog.Name = "ClearLog";
            ClearLog.Size = new Size(75, 25);
            ClearLog.TabIndex = 10;
            ClearLog.Text = "Clear";
            ClearLog.UseVisualStyleBackColor = true;
            ClearLog.Click += ClearLog_Click;
            // 
            // UseLanguage
            // 
            UseLanguage.AutoSize = true;
            UseLanguage.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            UseLanguage.Location = new Point(30, 158);
            UseLanguage.Name = "UseLanguage";
            UseLanguage.Size = new Size(93, 26);
            UseLanguage.TabIndex = 11;
            UseLanguage.Text = "是否翻译";
            UseLanguage.UseVisualStyleBackColor = true;
            UseLanguage.CheckedChanged += UseLanguage_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(210, 163);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 13;
            label1.Text = "导出配置标记";
            // 
            // BuildSign
            // 
            BuildSign.FormattingEnabled = true;
            BuildSign.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            BuildSign.Location = new Point(172, 158);
            BuildSign.Name = "BuildSign";
            BuildSign.Size = new Size(32, 25);
            BuildSign.TabIndex = 14;
            BuildSign.Text = "0";
            BuildSign.SelectedIndexChanged += BuildSign_SelectedIndexChanged;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 473);
            Controls.Add(BuildSign);
            Controls.Add(label1);
            Controls.Add(UseLanguage);
            Controls.Add(ClearLog);
            Controls.Add(ExcelPathError);
            Controls.Add(LogPanel);
            Controls.Add(BuildProgressBar);
            Controls.Add(BinaryButton);
            Controls.Add(ScriptableObjectButton);
            Controls.Add(SelectExcelPath);
            Controls.Add(ExcelPath);
            Name = "MainWindow";
            Text = "窗口";
            LogPanel.ResumeLayout(false);
            LogPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ExcelPath;
        private Button SelectExcelPath;
        private Button ScriptableObjectButton;
        private Button BinaryButton;
        private ProgressBar BuildProgressBar;
        private Panel LogPanel;
        private Label LogLabel;
        private LinkLabel ExcelPathError;
        private Button ClearLog;
        private CheckBox UseLanguage;
        private Label label1;
        private ComboBox BuildSign;
    }
}