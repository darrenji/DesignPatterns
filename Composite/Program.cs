using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            #region 一个简单例子
            //var graphicGroup = new GraphicObject();
            //graphicGroup.Children.Add(new Circle { Color = "Red" });
            //graphicGroup.Children.Add(new Square { Color = "Blue" });

            //Console.WriteLine(graphicGroup.ToString());
            #endregion

            #region 复杂一点的例子
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            var layer1 = new NeruonLayer();
            var layer2 = new NeruonLayer();

            neuron1.ConnectTo(neuron2);
            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(layer2);
            #endregion
        }
    }

    #region 一个简单例子
    public class GraphicObject
    {
        public string Color;
        public virtual string Name => "Group";

        private Lazy<List<GraphicObject>> _children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => _children.Value;

        public string Print(StringBuilder sb, int depth)
        {

            sb.Append(new string('*', depth))
                .Append($"当前图形的名称是{Name},")
                .AppendLine(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"当前图形的颜色是{Color}");
            foreach (var child in Children)
                child.Print(sb, depth + 1);
            return sb.ToString();
        }

        public override string ToString()
        {
            return Print(new StringBuilder(),0);
        }

    } 

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }
    #endregion

    #region 复杂一点的例子
    //模拟神经元
    //神经元和神经元之间相互联系,对其它神经元有引用，所以也是一个集合
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    //神经元层是神经元的一个集合
    public class NeruonLayer : Collection<Neuron> { }

    //神经元和神经元，神经元和神经元层之间需要连接
    public static class MyExtensions
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;
            foreach(var s in self)
            foreach(var o in other)
            {
                    s.Out.Add(o);
                    o.In.Add(s);
            }
        }
    }
    #endregion
}