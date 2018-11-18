using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class ProgressReport
    {
        /// <summary>
        /// 進捗パーセント
        /// </summary>
        public int Percent;

        /// <summary>
        /// 推定残り時間
        /// </summary>
        public TimeSpan EstimatedRemain;
    }
}
