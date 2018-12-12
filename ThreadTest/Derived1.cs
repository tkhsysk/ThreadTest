using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    internal class Derived1 : Base
    {
        internal override string Text { get { return "derived1"; } }

        // 静的な型によって、どのメソッドが呼び出されるかが決まる
        internal void Test()
        {
            Debug.WriteLine("derived1");
        }
    }
}
