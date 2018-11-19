using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class SensorData
    {
        public event EventHandler RecievedData;
        public delegate void RequestDataAsync();

        public void RequestData()
        {
            Thread.Sleep(500);

            if(RecievedData != null)
            {
                RecievedData(this, EventArgs.Empty);
            }
        }
    }
}
