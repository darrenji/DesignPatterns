using System;
using System.Collections.Generic;
using System.Text;

namespace Strategy
{
    class Program
    {
        static void Main(string[] args)
        {
            #region dynamic strategy，在运行时设置
            //var tp = new TextProcessor();
            //tp.SetOutputFormat(OutputFormat.Html);
            //tp.AppendList(new[] { "foo", "bar", "baz" });
            //Console.WriteLine(tp); 
            #endregion

            #region static strategy
            var tp = new TextProcessor1<HtmlListStrategy>();
            tp.AppendList(new[] { "foo","bar","baz"});
            Console.WriteLine(tp);
            #endregion
        }
    }

    #region dynamic strategy
    public enum OutputFormat
    {
        Markdown,Html
    }

    //<ul><li>foo</li></ul>
    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddListItem(StringBuilder sb, string item);
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" <li>{item}</li>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" *{item}");
        }

        public void End(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public void Start(StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }

    public class TextProcessor
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy;

        public void SetOutputFormat(OutputFormat format)
        {
            switch(format)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    listStrategy = new HtmlListStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(sb);
            foreach (var item in items)
                listStrategy.AddListItem(sb, item);
            listStrategy.End(sb);
        }

        public StringBuilder Clear()
        {
            return sb.Clear();
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
    #endregion

    #region static strategy
    public class TextProcessor1<T> where T:IListStrategy,new()
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy = new T();

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(sb);
            foreach (var item in items)
                listStrategy.AddListItem(sb, item);
            listStrategy.End(sb);
        }

        public StringBuilder Clear()
        {
            return sb.Clear();
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
    #endregion
}