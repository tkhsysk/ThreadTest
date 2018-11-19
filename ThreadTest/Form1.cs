using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadTest
{
    public partial class Form1 : Form
    {
        private const int SAY_COUNT = 5;
        private const int WAIT＿MILLISECONDS = 1000;

        private CancellationTokenSource _cts;
        private System.Threading.Timer _timer;

        private delegate string SayDelegate(string word);

        #region constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region method

        /// <summary>
        /// しゃべる
        /// </summary>
        /// <param name="word"></param>
        public void Say(object word)
        {
            for (int i = 0; i < SAY_COUNT; i++)
            {
                Debug.WriteLine($"{word.ToString()}!!!");
                Thread.Sleep(WAIT＿MILLISECONDS);
            }
        }

        /// <summary>
        /// しゃべる
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Say(string word)
        {
            for (int i = 0; i < SAY_COUNT; i++)
            {
                Debug.WriteLine($"{word.ToString()}!!!");
                Thread.Sleep(WAIT＿MILLISECONDS);
            }

            return "ok";
        }

        /// <summary>
        /// こんにちは
        /// </summary>
        public void SayHello()
        {
            for (int i = 0; i < SAY_COUNT; i++)
            {
                Debug.WriteLine($"hello!!!");
                Thread.Sleep(WAIT＿MILLISECONDS);
            }
        }

        /// <summary>
        /// コールバック関数
        /// </summary>
        /// <param name="ar"></param>
        public void SayComplete(IAsyncResult ar)
        {
            SayDelegate d = (SayDelegate)((AsyncResult)ar).AsyncDelegate;
            string returnValue = d.EndInvoke(ar);

            // callbackに渡されるパラメータ
            DateTime now = (DateTime)ar.AsyncState;

            // say "ok"
            Debug.WriteLine($"{returnValue} {now.ToString()}");
        }

        /// <summary>
        /// UIを更新する
        /// </summary>
        /// <param name="report"></param>
        public void SetProgressValue(ProgressReport report)
        {
            progressBar1.Value = report.Percent;
            if(0 < report.Percent && report.Percent < 100)
            {
                label1.Text = string.Format("あと {0} 秒...", report.EstimatedRemain.TotalSeconds);
            }
        }

        public async Task DoWorkAsync(IProgress<ProgressReport> progress, CancellationToken token)
        {
            const int COUNT = 10;
            const int PERIOD_MSEC = 500;

            for(int i = 0; i < COUNT; i++)
            {
                await Task.Delay(PERIOD_MSEC);

                if (token.IsCancellationRequested)
                {
                    // 中断するための処理を記述する
                    Thread.Sleep(1000);

                    token.ThrowIfCancellationRequested();
                }

                progress.Report(new ProgressReport()
                {
                    Percent = (i + 1) * COUNT,
                    EstimatedRemain = TimeSpan.FromMilliseconds((COUNT - 1 - i) * PERIOD_MSEC)
                });
            }
        }

        #endregion

        #region handler

        #region Threadを使う

        /// <summary>
        /// スレッドを起動（パラメータを渡さない）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // スレッドにパラメータを渡さない
            Thread t = new Thread(new ThreadStart(SayHello));

            // スレッドを起動
            t.Start();

            // スレッドの終了を待つ
            t.Join();
        }

        /// <summary>
        /// スレッドを起動（パラメータを渡す）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(Say));

            // パラメータを渡して、スレッドを起動
            t.Start("bye");
        }

        #endregion

        #region ThreadPool

        /// <summary>
        /// スレッドプールのスレッドを起動（パラメータを渡す）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            // パラメータを渡して、スレッドプールのスレッドを起動
            bool queResult = ThreadPool.QueueUserWorkItem(new WaitCallback(Say), "bye");
        }

        #endregion

        #region delegate

        /// <summary>
        /// delegateを使ってスレッドを起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            // delegateの作成
            SayDelegate d = new SayDelegate(Say);

            // BeginInvokeの呼び出し
            IAsyncResult ar = d.BeginInvoke("hello", new AsyncCallback(SayComplete), DateTime.Now);

            // 参考サイト:
            // http://www.atmarkit.co.jp/ait/articles/0504/20/news111_3.html
        }

        #endregion

        #region task

        /// <summary>
        /// タスクを使う（ThreadPool）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Task task = new Task(() =>
            {
                SayHello();
            });

            task.Start();
        }

        /// <summary>
        /// タスクの完了を待つ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(new Action(() => Say("task1")));
            tasks[1] = Task.Factory.StartNew(new Action(() => Say("task2")));

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// タスクの結果を取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            Task<string> task = new Task<string>(() => Say("hello"));
            task.Start();

            // タスクが完了するまで、wait
            string result = task.Result;

            Debug.WriteLine(result);
        }

        /// <summary>
        /// 継続タスクを使う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            Task task = new Task(SayHello);

            // t: task
            // callback
            task.ContinueWith((t) => Debug.WriteLine(DateTime.Now.ToString()));
            task.Start();
        }


        #endregion

        #region async/await

        /// <summary>
        /// async/awaitを使う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button9_Click(object sender, EventArgs e)
        {
            string result = await Task.Run<string>(() => Say("hello"));
            Debug.WriteLine(result);
        }

        #endregion

        #region 進捗更新

        /// <summary>
        /// スタート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button10_Click(object sender, EventArgs e)
        {
            button10.Enabled = false;
            button11.Enabled = true;

            label1.ForeColor = Color.DodgerBlue;
            label1.Text = "実行中...";

            var progress = new Progress<ProgressReport>(SetProgressValue);

            // キャンセルトークンを作成
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            try
            {
                await DoWorkAsync(progress, token);

                label1.ForeColor = Color.DarkRed;
                label1.Text = "完了";
            }
            catch (OperationCanceledException)
            {
                label1.ForeColor = Color.DarkRed;
                label1.Text = "中断";
            }

            button10.Enabled = true;
            button11.Enabled = false;
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            _cts.Cancel();

            button11.Enabled = false;
            label1.ForeColor = Color.DarkOrange;
            label1.Text = "中断中...";
        }


        #endregion

        #region utility

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            // 拡張メソッド
            //Debug.WriteLine(100.AllwaysZero());

            // メソッドを返す
            //var ja = Utility.GetMessageCreator("ja");
            //Debug.WriteLine(ja("ケン"));

            // デバッガ
            // https://www.ryobi.co.jp/column/visual-studio-tips1

            // トレースポイント
            // コードを変えずに、ログを出力できる
            string[] list = { "test", "test2", "test3" };
            foreach(var l in list)
            {
                Debug.WriteLine(l);
            }

            // イミディエイトウィンドウ
            // http://troushoo.blog.fc2.com/blog-entry-177.html
        }

        #region 継承

        private void button13_Click(object sender, EventArgs e)
        {
            // http://www.atmarkit.co.jp/fdotnet/csharp_abc/csharp_abc_004/csharp_abc02.html

            Person person = new Person();
            Debug.WriteLine(person.GetName());  // なし
            Debug.WriteLine(person.Name);

            Taro taro = new Taro();
            Debug.WriteLine(taro.GetName());     // Taro
            Debug.WriteLine(taro.Name);

            Person someone = new Taro();
            Debug.WriteLine(someone.GetName()); // Taro
            Debug.WriteLine(someone.Name);

            // abstract
            APerson asomeone = new Rena();
            Debug.WriteLine(asomeone.GetName());    // Rena

            // interface
            IPerson isomeone = new Ken();
            Debug.WriteLine(isomeone.GetName());    // Ken

            JankenValue[] jv = { JankenValue.Goo, JankenValue.Choki, JankenValue.Par };
            Debug.WriteLine(jv[0]); // Goo

            JankenValue temp = (JankenValue)Enum.Parse(typeof(JankenValue), "Goo");
            Debug.WriteLine(temp);  // Goo
        }

        #endregion

        #region timer

        private void ThreadingTimerCallback(object state)
        {
            // 一時中断
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            SensorData data = state as SensorData;

            // 同期
            //data.RequestData();

            // 非同期
            Task task = new Task(data.RequestData);
            task.Start();

            int id = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine($"{ DateTime.Now.ToString()} {id}");

            // 再開
            _timer.Change(1000, 0);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SensorData data = new SensorData();
            data.RecievedData += Data_RecievedData;

            _timer = new System.Threading.Timer(new TimerCallback(ThreadingTimerCallback), data, 0, 1000);
        }

        private void Data_RecievedData(object sender, EventArgs e)
        {
            Debug.WriteLine("recieved.");
        }

        #endregion

        #endregion

        #endregion


    }
}
