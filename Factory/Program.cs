using System;
using System.Collections.Generic;

namespace Factory
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 内部工厂
            //Point2.Factory.NewCartisianPoint(1,2);
            //Point2.Factory.NewPolarPoint(1,2);

            #endregion

            #region 抽象工厂
            //IHotDrinkFactory factory = new TeaFactory();
            //factory.Produce().Consume();
            #endregion

            #region 抽象工厂与反射
            var hotDrinkMachine = new HotDrinkMachine();
            hotDrinkMachine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea).Consume();
            #endregion
        }
    }

    #region 工厂方法

    public class Point
    {
        private double x, y;

        //私有构造函数
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point NewCartisanPoint(double x, double y)
        {
            return new  Point(x, y);
        }

        public static Point NewPolarPoint(double x, double y)
        {
            return new Point(x * Math.Cos(y), x * Math.Sin(y));
        }
    }

    #endregion

    #region 具体工厂
    public class Point1
    {
        private double x, y;
        public Point1(double x, double y)
        {
            x = x;
            y = y;
        }
    }

    public static class Point1Factory
    {
        public static Point1 NewCartisianPoint(double x, double y)
        {
            return new Point1(x, y);
        }

        public static Point1 NewPolarPoint(double x,double y)
        {
            return new Point1(x * Math.Cos(y), x * Math.Sin(y));
        }
    }
    #endregion

    #region 内部工厂
    public  class Point2
    {
        private double a, b;
        private  Point2(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public static Point2Factory Factory => new Point2Factory();

        public class Point2Factory
        {
            public  Point2 NewCartisianPoint(double a, double b)
            {
                return new Point2(a, b);
            }

            public  Point2 NewPolarPoint(double a,double b)
            {
                return new Point2(a * Math.Cos(b), a * Math.Sin(b));
            }
        }
    }
    #endregion

    #region 抽象工厂
    public interface IHotDrink
    {
        void Consume();
    }

    public class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("drink tea everyday");
        }
    }

    public class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("drink coffee every morning");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Produce();
    }

    public class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Produce()
        {
            return new Tea();
        }
    }

    public class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Produce()
        {
            return new Coffee();
        }
    }
    #endregion

    #region 抽象工厂与反射
    public class HotDrinkMachine
    {
        public enum AvailableDrink
        {
            //这两个枚举值需要和具体工厂的名称对应
            Coffee,Tea
        }

        private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach(AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                //利用反射来实例化工厂
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("Factory." + Enum.GetName(typeof(AvailableDrink),drink) + "Factory")
                );

                factories.Add(drink,factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink)
        {
            return factories[drink].Produce();
        }
    }
    #endregion
}