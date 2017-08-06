using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyweight
{
    class Program
    {
        static void Main(string[] args)
        {
            var ft = new FormattedText("this is a new world");
            ft.Capitalize(5, 8);
            Console.WriteLine(ft.ToString());

            var bft = new BetterFormattedText("this is a new world");
            bft.GetRange(5, 8).Capitalize = true;
            Console.WriteLine(bft);
        }
    }

    #region 1
    public class User
    {
        private string fullName;
        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }

    public class User2
    {
        static List<string> strings = new List<string>(); //名字字符串，以空格
        private int[] names; //存储索引位置


        public User2(string fullName)
        {
            //方法中的方法,获取某一个元素的索引
            int getOrAdd(string s)
            {

                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullName.Split(' ').Select(getOrAdd).ToArray();


        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));
    }
    #endregion

    #region Text Formatting

    //正常的写法
    public class FormattedText
    {
        private readonly string _plainText;

        //把与字符串对应的映射放在一个数组里，这种写法很有意思
        private bool[] _capitalize;//bool类型的数组，如果元素值为true，那就需要转换成大写
        public FormattedText(string plainText)
        {
            _plainText = plainText;
            _capitalize = new bool[plainText.Length];
        }

        //选择某一个位置区域内大写
        public void Capitalize(int start, int end)
        {
            for(int i=start;i<=end;i++)
            {
                _capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(var i=0;i<_plainText.Length;i++)
            {
                var c = _plainText[i];
                sb.Append(_capitalize[i] ?char.ToUpper(c):c);
            }
            return sb.ToString();
        }
    }

    //更好的写法
    public class BetterFormattedText
    {
        private string _plainText;
        private List<TextRange> _formatting = new List<TextRange>();

        public BetterFormattedText(string plainText)
        {
            _plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            //把符合条件的实例存放到实例集合中来
            _formatting.Add(range);
            return range;
        }

        //把生成的实例设计成一个内部类，会有更低的内存开销
        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            //判断当前位置在不在范围内
            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(var i=0;i<_plainText.Length;i++)
            {
                var c = _plainText[i];
                foreach(var range in _formatting)
                {
                    if(range.Covers(i)&&range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
    #endregion
}