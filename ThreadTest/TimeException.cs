using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class TimeException : Exception
    {
        public Reason Reason { get; set; }

        public TimeException(Reason reason, string message)
        {
            Reason = reason;
        }
    }

    public enum Reason : int
    {
        Timeout,
        None
    }
}
