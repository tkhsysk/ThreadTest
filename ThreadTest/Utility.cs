using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    public static class Utility
    {
        /// <summary>
        /// 可変個引数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int Average(params int[] nums)
        {
            return nums.Length == 0 ? 0 : (int)nums.Average();
        }

        /// <summary>
        /// 省略可能な引数、引数の既定値
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void SayHello(string name, string message = "hello")
        {
            Debug.WriteLine($"{message}, {name}!");
        }
        
        /// <summary>
        /// 拡張メソッド、自プロジェクトのクラスに適用すべきではない
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int AllwaysZero(this int n)
        {
            return 0;
        }

        /// <summary>
        /// 関数を返すメソッド
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static Func<string, string> GetMessageCreator(string lang)
        {
            if(lang == "en")
            {
                return CreateMessageEn;
            }
            else if(lang == "ja")
            {
                return CreateMessageJa;
            }
            else if(lang == null)
            {
                return name => string.Empty;
            }
            else
            {
                throw new ArgumentException("wrong lang: " + lang);
            }
        }

        /// <summary>
        /// 英語
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string CreateMessageEn(string name)
        {
            return $"hello, {name}!";
        }

        /// <summary>
        /// 日本語
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string CreateMessageJa(string name)
        {
            return $"こんにちは, {name}!";
        }

    }
}
