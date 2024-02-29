using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools.UI
{
    internal class BuildProgress
    {
        ProgressBar bar;
        int task;
        bool begin;
        public BuildProgress(ProgressBar bar)
        {
            this.bar = bar;
        }
        public void Init(int taskCount)
        {
            task = taskCount;
            begin = true;
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
            Debug.Log("进度条完成");
        }
    }
}
