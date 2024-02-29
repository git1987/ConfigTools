using System.Text;
using ConfigTools.UI;
using ConfigTools.DataType;
using Timers = System.Windows.Forms.Timer;

namespace ConfigTools
{
    public partial class MainWindow : Form
    {
        public static MainWindow instance;
        StringBuilder logSB = new();
        BuildProgress progress;
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            ClearLog_Click(null, null);
            string relativePath = AppDomain.CurrentDomain.BaseDirectory;
            Debug.Log(relativePath);
            LogPanel.AutoScroll = true;
            LogPanel.Controls.Add(LogLabel);
            progress = new BuildProgress(BuildProgressBar);
            //
            Config.Init();
            ExcelPath.Text = Config.readExcelPath;
            BuildSign.Text = Config.outputType;
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
        //excel path
        private void ExcelPath_TextChanged(object sender, EventArgs e)
        {
            BinaryButton.Enabled =
                ScriptableObjectButton.Enabled =
                Directory.Exists(ExcelPath.Text);
            if (Directory.Exists(ExcelPath.Text))
            {
                ExcelPathError.Text = string.Empty;
            }
            else
            {
                ExcelPathError.Text = "Excel Path Error!";
            }
        }
        //select excel path
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
            AddLog($"build ScriptableObject, is Language:{UseLanguage.Checked}");
            ObjectType type = new ScriptableObject();
            type.ReadExcels(ExcelPath.Text);
        }

        private void BinaryButton_Click(object sender, EventArgs e)
        {
            AddLog("build Binary");
            ObjectType type = new Binary();
            type.ReadExcels(ExcelPath.Text);
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            logSB.Clear();
            AddLog("Log:");
        }

        private void BuildSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.outputType = BuildSign.Text;
        }

        private void UseLanguage_CheckedChanged(object sender, EventArgs e)
        {
            Config.isLanguage = UseLanguage.Checked;
        }
    }
}