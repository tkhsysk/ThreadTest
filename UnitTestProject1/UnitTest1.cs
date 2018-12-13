using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadTest;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        // Initialize
        // Cleanup
        // テストメソッドを修正したら、ビルドする必要ある

        // グループ化
        // TestCategory

        // コードカバレッジの目安80%くらい

        [TestMethod]
        public void TestMethod1()
        {
            Stationery stationery = new Stationery();

            Assert.AreEqual(1, stationery.GetCounts());
        }
    }
}
