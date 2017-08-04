using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Adapter
{
    class Program
    {
        private static readonly List<VectorObject> vectorObjects = new List<VectorObject> {
             new VectorRectangle(1, 1, 10, 10),
             new VectorRectangle(3, 3, 6, 6)
        };

        public static void DrawPoin(Point point)
        {
            Console.Write(".");
        }

        public static void DrawPoints()
        {
            foreach(var vo in vectorObjects)
            {
                foreach(var line in vo)
                {
                    var points = new LineToPointAdapter(line);
                    foreach(var point in points)
                    {
                        DrawPoin(point);
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DrawPoints();
            DrawPoints();
        }
    }

    //点
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }

    //线,2个点就是一条线
    public class Line
    {
        public Point Start, End;

        public Line(Point start, Point end)
        {
            Start = start ?? throw new ArgumentNullException(paramName: nameof(start));
            End = end ?? throw new ArgumentNullException(paramName: nameof(end));
        }

        protected bool Equals(Line other)
        {
            return Equals(Start, other.Start) && Equals(End, other.End);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Line)obj);
        }

        public override int GetHashCode()
        {
            return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
        }
    }

    //向量，是由线构成，是线的集合
    public class VectorObject : Collection<Line>
    {
        
    }

    //向量有很多派生类
    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x,y), new Point(x+width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y+height)));
            Add(new Line(new Point(x, y+height), new Point(x + width, y+height)));
        }
    }

    //线是由点构成的，写一个适配器把线转换成点
    public class LineToPointAdapter : IEnumerable<Point>
    {

        private static int count;

        //缓冲，键为hash值
        static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>();
        //在构造函数中转换
        public LineToPointAdapter(Line line)
        {
            var lineHash = line.GetHashCode();
            if (cache.ContainsKey(lineHash))
                return;

            Console.WriteLine($"{++count}正在把起点为[{line.Start.X},{line.Start.Y}]终点为[{line.End.X},{line.End.Y}]的线转换成点");

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            var points = new List<Point>();

            //如果是竖线
            if(dx==0)
            {
                for(int y=top;y<=bottom;++y)
                {
                    points.Add(new Point(left, y));
                }
            }
            else if(dy==0)//如果是横线
            {
                for(int x=left;x<=right;++x)
                {
                    points.Add(new Point(x, top));
                }
            }

            cache.Add(lineHash, points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}