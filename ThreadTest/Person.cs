using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    public enum JankenValue
    {
        Goo,
        Choki,
        Par
    }

    // プロパティ、メソッドをoverride

    public class Person
    {
        public virtual string Name { get; set; } = "なし";

        public virtual string GetName()
        {
            return "名前は、なしです。";
        }
    }

    /// <summary>
    /// 抽象クラス
    /// </summary>
    public abstract class APerson
    {
        public abstract string GetName();
    }

    /// <summary>
    /// インターフェース
    /// </summary>
    public interface IPerson
    {
        string GetName();
    }

    public class Taro : Person
    {
        public override string Name { get; set; } = "Taro";

        public override string GetName()
        {
            // return base.GetName();
            return "名前は、Taroです。";
        }
    }

    public class Rena : APerson
    {
        public override string GetName()
        {
            return "名前は、Renaです。";
        }
    }

    public class Ken : IPerson
    {
        public string GetName()
        {
            return "名前は、Kenです。";
        }
    }

}
