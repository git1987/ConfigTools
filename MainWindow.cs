using System;
using System.Diagnostics;
using System.Text;
using ConfigTools.ScriptableObject;
using ConfigTools.UI;
using Microsoft.VisualBasic.Logging;
using Timers = System.Windows.Forms.Timer;
namespace ConfigTools
{
    public partial class MainWindow : Form
    {
        public static MainWindow instance;
        StringBuilder logSB = new("日志：\n");
        BuildProgress progress;
        public ConfigEnum configEnum;
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            LogPanel.AutoScroll = true;
            LogPanel.Controls.Add(LogLabel);
            progress = new BuildProgress(BuildProgressBar);
            //初始化配置文件
            Config.Init();
            ExcelPath.Text = Config.readExcelPath;
            OutputPath.Text = Config.unityPath;
            //test
            Timers timer = new Timers() { Interval = 1 };
            int temp = 0;
            progress.Init(10);
            timer.Tick += (obj, e) =>
            {
                temp++;
                progress.Update(temp);
            };
            timer.Start();
        }
        //excel路径
        private void ExcelPath_TextChanged(object sender, EventArgs e)
        {
            BinaryButton.Enabled =
                ScriptableObjectButton.Enabled =
                Directory.Exists(ExcelPath.Text) &&
                Directory.Exists(OutputPath.Text);
            if (Directory.Exists(ExcelPath.Text))
            {
                ExcelPathError.Text = string.Empty;
            }
            else
            {
                ExcelPathError.Text = "路径错误";
            }
        }
        //选择excel路径按钮
        private void SelectExcelPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog open = new FolderBrowserDialog())
            {
                open.InitialDirectory = Environment.CurrentDirectory;
                DialogResult result = open.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ExcelPath.Text = open.SelectedPath;
                    AddLog(open.SelectedPath);
                }
            }
        }
        //配置路径
        private void OutputPath_TextChanged(object sender, EventArgs e)
        {
            BinaryButton.Enabled =
                ScriptableObjectButton.Enabled =
                Directory.Exists(ExcelPath.Text) &&
                Directory.Exists(OutputPath.Text);
            if (Directory.Exists(OutputPath.Text))
            {
                OutputPathError.Text = string.Empty;
            }
            else
            {
                OutputPathError.Text = "路径错误";
            }
        }
        //导出配置路径按钮
        private void OutputButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog open = new FolderBrowserDialog())
            {
                open.InitialDirectory = Environment.CurrentDirectory;
                DialogResult result = open.ShowDialog();
                if (result == DialogResult.OK)
                {
                    OutputPath.Text = open.SelectedPath;
                    AddLog(open.SelectedPath);
                }
            }
        }

        public void AddLog(string content)
        {
            logSB.AppendLine(content);
            LogLabel.Text = logSB.ToString();
            LogPanel.AutoScrollPosition =
                new Point(0, LogPanel.Size.Height);
        }
        private void LogPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void ScriptableObjectButton_Click(object sender, EventArgs e)
        {
            configEnum = new();
            AddLog($"输出ScriptableObject,是否翻译：{UseLanguage.Checked}");
            ScriptableObjectType type = new ScriptableObjectType();
            type.ReadExcels(ExcelPath.Text, UseLanguage.Checked);
        }

        private void BinaryButton_Click(object sender, EventArgs e)
        {
            configEnum = new();
            AddLog("输出二进制");

        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            logSB.Clear();
            AddLog("日志：");
        }

    }
}