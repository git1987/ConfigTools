
namespace ConfigTools.UI
{
    internal class BuildProgress
    {
        public static string ProgressUpdate = "ProgressUpdate";
        public static string ProgressFinish = "ProgressFinish";
        ProgressBar bar;
        int task;
        bool begin;
        public BuildProgress(ProgressBar bar)
        {
            this.bar = bar;
            EventManager<int>.AddListener(typeof(BuildProgress).Name, Init);
        }
        public void Init(int taskCount)
        {
            task = taskCount;
            begin = true;
            EventManager<int>.AddListener(ProgressUpdate, Update);
            EventManager.AddListener(ProgressFinish, Finish);
        }
        public void Update(int currentTask)
        {
            if (begin)
            {
                int value = (int)(currentTask / task);
                if (value >= 100)
                {
                    Finish();
                }
                else
                {
                    bar.Value = value;
                }
            }
        }
        public void Finish()
        {
            bar.Value = 100;
            begin = false;
            Debug.Log("导出配置完成");
            EventManager<int>.RemoveListener(ProgressUpdate, Update);
        }
    }
}
