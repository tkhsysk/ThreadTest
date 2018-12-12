using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    internal class Base
    {
        internal virtual string Text { get { return "base"; } }

        internal void Test()
        {
            Debug.WriteLine("Base");
        }
    }
}
