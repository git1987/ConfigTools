using System.Text;
using ConfigTools.UI;
using ConfigTools.ScriptableObject;
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
            LogPanel.AutoScroll = true;
            LogPanel.Controls.Add(LogLabel);
            progress = new BuildProgress(BuildProgressBar);
            //
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
        //excel path
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
        //output path
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
                OutputPathError.Text = "Output Path Error!";
            }
        }
        //select output path
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
            AddLog($"build ScriptableObject, is Language£º{UseLanguage.Checked}");
            ScriptableObjectType type = new ScriptableObjectType();
            type.ReadExcels(ExcelPath.Text, UseLanguage.Checked);
        }

        private void BinaryButton_Click(object sender, EventArgs e)
        {
            AddLog("build Binary");

        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            logSB.Clear();
            AddLog("Log:");
        }

    }
}