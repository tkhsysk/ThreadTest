using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadTest;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        #region データ駆動を実行する際に必要

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #endregion

        // Initialize
        // Cleanup
        // テストメソッドを修正したら、ビルドする必要ある

        // グループ化
        // TestCategory

        // コードカバレッジの目安80%くらい

        [TestMethod]
        [TestCategory("正常系テスト")]
        public void TestMethod1()
        {
            Stationery stationery = new Stationery();

            Assert.AreEqual(1, stationery.GetCounts());
        }

        [TestMethod]
        [TestCategory("異常系テスト")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"test.csv", "test#csv", DataAccessMethod.Sequential)]
        public void TestMethod2()
        {
            // https://qiita.com/mima_ita/items/55394bcc851eb8b6dc24
            // test.csvは、sjisで保存すること

            // https://docs.microsoft.com/ja-jp/visualstudio/test/how-to-create-a-data-driven-unit-test?view=vs-2015
            // データ ドリブン テストのデータ ソース情報を格納する TestContext オブジェクトを作成します。 
            //次に、フレームワークは作成する TestContext プロパティの値としてこのオブジェクトを設定します。

            int a = (int)TestContext.DataRow["a"];
            int b = (int)TestContext.DataRow["b"];
            int result = (int)TestContext.DataRow["result"];

            Debug.WriteLine(a + " " + b + " " + result);
            Assert.AreEqual(result, a + b);
        }

        [TestMethod]
        public void TestMethod3()
        {
            Debug.WriteLine((int)Animal.Dog);
            Debug.WriteLine((int)Animal.Cat);
            Debug.WriteLine((int)Animal.Bird);
        }

        [TestMethod]
        public void TestMethod4()
        {
            // chainig assertion
            // private method

            SensorData data = new SensorData();
            (data.AsDynamic().GetSensorName() as string).Is("sensor1");
        }

        [TestMethod]
        [TestCase(1, 2, 13)]
        public void TestMethod5()
        {
            TestContext.Run((int x, int y, int z) =>
            {
                (x + y).Is(z);
            });
        }
    }
}
