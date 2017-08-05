using System;
using System.Text;


namespace Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            #region 简单例子
            //var pingDiGuo = new Product("平底锅", 100);
            //var addJiangYou = new ProductDecorator("酱油", 10, pingDiGuo);
            //var addSuanCai = new ProductDecorator("老坛酸菜", 20, addJiangYou);

            //Console.WriteLine(addSuanCai.ShowProductInfo());
            #endregion

            #region Multiple Inheritance
            //var dragon = new Dragon();
            //dragon.Weight = 100;
            //dragon.Fly();
            //dragon.Crawl();
            #endregion

            #region Dynamic Decorator
            //var square = new Square(1.23f);
            //Console.WriteLine(square.AsString());

            //var redSquare = new ColorShape(square, "红色");
            //Console.WriteLine(redSquare.AsString());
            #endregion

            #region Static Decorator
            var colorShape = new ColoredShape<Square1>("红色");
            Console.WriteLine(colorShape.AsString());

            var circle = new TransparentShape<ColoredShape<Circle1>>(0.4f);
            Console.WriteLine(circle.AsString());
            #endregion
        }
    }

    #region 简单例子,针对一个抽象基类的Decorator
    //被装饰的对象需要一个抽象基类
    public abstract class ProductBase
    {
        public abstract string GetName();
        public abstract double GetPrice();
    }

    //被装饰的对象抽象基类需要一个实现类
    public class Product : ProductBase
    {
        private readonly string _name;
        private readonly double _price;

        public Product(string name, double price)
        {
            _name = name;
            _price = price;
        }

        public override string GetName()
        {
            return _name;
        }

        public override double GetPrice()
        {
            return _price;
        }
    }

    //装饰器类在抽象基类的基础上增加新的功能
    public class ProductDecorator : ProductBase
    {
        private readonly string _name;
        private readonly double _price;
        private ProductBase _productBase;

        public ProductDecorator(string name, double price, ProductBase productBase)
        {
            _name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            _price = price;
            _productBase = productBase ?? throw new ArgumentNullException(paramName: nameof(productBase));
        }
        public override string GetName()
        {
           return  $"{_name},{_productBase.GetName()}";
        }

        public override double GetPrice()
        {
            return _price + _productBase.GetPrice();
        }

        //新增加的方法
        public string ShowProductInfo()
        {
            return $"产品包括：[{GetName()}],总价为：{GetPrice().ToString()}";
        }
    }

    #endregion

    #region 自定义一个StringBuilder，针对一个具体类的Decorator
    //public class CodeBuilder
    //{
    //    private StringBuilder builder = new StringBuilder();

    //    public override string ToString()
    //    {
    //        return builder.ToString();
    //    }

    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        ((ISerializable)builder).GetObjectData(info, context);
    //    }

    //    public int EnsureCapacity(int capaicty)
    //    {
    //        return builder.EnsureCapacity(capaicty);
    //    }

    //    public string ToString(int startIndex, int length)
    //    {
    //        return builder.ToString(startIndex, length);
    //    }

    //    public CodeBuilder Clear()
    //    {
    //        builder.Clear();
    //        return this;
    //    }

    //    public CodeBuilder Append(char value, int repeatCount)
    //    {
    //        builder.Append(value, repeatCount);
    //        return this;
    //    }
    //}
    #endregion

    #region Multiple Inheritance，针对多个接口的Decorator
    public interface IBird
    {
        void Fly();
        int Weight { get; set; }
    }

    public class Bird : IBird
    {
        public int Weight { get; set; }

        public void Fly()
        {
            Console.WriteLine($"在空中飞翔,体重是{Weight}");
        }
    }

    public interface ILizard
    {
        void Crawl();
        int Weight { get; set; }
    }

    public class Lizard : ILizard
    {
        public int Weight { get; set; }
        public void Crawl()
        {
            Console.WriteLine($"在地上爬行,体重是{Weight}");
        }
    }

    public class Dragon : IBird, ILizard
    {
        private Bird bird = new Bird();
        private Lizard lizard = new Lizard();

        //这里处理起来不方便
        public int Weight
        {
            get { return bird.Weight; }
            set
            {
                bird.Weight = value;
                lizard.Weight = value;
            }
        }

        public void Crawl()
        {
            lizard.Crawl();
        }

        public void Fly()
        {
            bird.Fly();
        }
    }
    #endregion

    #region Dynamic Decorator，针对一个接口的Decorator
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float _radius;


        public Circle(float radius)
        {
            _radius = radius;
        }

        public string AsString() => $"一个圆，半径是{_radius}";

        public void Resize(float factor)
        {
            _radius *= factor;
        }
    }

    public class Square : IShape
    {
        private float _side;



        public Square(float side)
        {
            _side = side;
        }
        public string AsString() => $"一个正方形，边长是{_side}";

    }

    //这里时Decorator
    public class ColorShape : IShape
    {
        private IShape _shape;
        private string _color;

        public ColorShape(IShape shape, string color)
        {
            _color = color ?? throw new ArgumentNullException(paramName: nameof(color));
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
        }

        public string AsString()
        {
            return $"{_shape.AsString()}的颜色是{_color}";
        }
    }
    #endregion

    #region Static Decorator
    public abstract class Shape
    {
        public abstract string AsString();
    }

    public class Circle1: Shape
    {
        private float _radius;

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        public Circle1():this(0)
        {

        }
        public Circle1(float radius)
        {
            _radius = radius;
        }

        public void Resize(float factor)
        {
            _radius *= factor;
        }

        public override string AsString()
        {
            return $"一个圆， 半径是{_radius}";
        }
    }

    public class Square1 : Shape
    {
        private float _side;

        public Square1():this(0.0f)
        {

        }
        public Square1(float side)
        {
            _side = side;
        }

        public override string AsString()
        {
            return $"一个正方形，边长是{_side}";
        }
    }


    //Decorator，针对抽象基类
    public class ColorShape1 : Shape
    {
        private Shape _shape;
        private string _color;
        public ColorShape1(Shape shape, string color)
        {
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            _color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }
        public override string AsString()
        {
            return $"{_shape.AsString()}，颜色是{_color}";
        }
    }

    //又一个Decorator,针对抽象基类
    public class TransparentShape:Shape
    {
        private Shape _shape;
        private float _transparency;

        public TransparentShape(Shape shape, float transparency)
        {
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            _transparency = transparency;
        }

        public override string AsString()
        {
            return $"{_shape.AsString()}透明度是{_transparency*100.00}";
        }
    }

    //泛型的Decorator
    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private string _color;
        private T _shape = new T();

        public ColoredShape() : this("黑色")
        {

        }

        public ColoredShape(string color)
        {
            _color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString()
        {
            return $"{_shape.AsString()}的颜色是{_color}";
        }
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private float _transparency;
        private T _shape = new T();

        public TransparentShape():this(0)
        {

        }

        public TransparentShape(float transparency)
        {
            _transparency = transparency;
        }
        public override string AsString()
        {
            return $"{_shape.AsString()}透明度是{_transparency * 100.0f}%";
        }
    }


    #endregion
}