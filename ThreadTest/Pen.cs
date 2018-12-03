using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    internal class Pen : Stationery
    {
        // ToString: virtual
        // overrideするためには、virtual、abstract、overrideに設定されている必要がある
        public override string ToString()
        {
            return "pen";
        }

        // new: 基底クラスの設定不要
        // http://gacken.com/wp/program/c-sharp/2692/
        internal new int GetCounts()
        {
            return 100;
        }
    }
}
