using System;
using System.Net.Http;

namespace NullObject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please select the Shipping method 1.FexEd 2.DHL 3.In House Shipping");
            string input = Console.ReadLine();

            int choice = 0;
            bool result = int.TryParse(input, out choice);

            if(result==true)
            {
                OrderProcessor orderProcessor = new OrderProcessor();

                //这里体现null object pattern
                //IShippingStrategy shippingStrategy = new InvalidShippingStrategy();//一般的写法
                IShippingStrategy shippingStrategy = InvalidShippingStrategy.Instance;

                switch (choice)
                {
                    case 1:
                        shippingStrategy = new FedExShippingStrategy();
                        break;
                    case 2:
                        shippingStrategy = new DHLShippingStrategh();
                        break;
                    case 3:
                        shippingStrategy = new InHouseShippingStrategy();
                        break;
                }
                orderProcessor.ProcessOrder(shippingStrategy);
            }
        }
    }

    //Null Object pattern让我们返回一个空的object而不是null
    #region NullObject
    public interface IShippingStrategy
    {
        void ScheduleShipping();
    }

    public class DHLShippingStrategh : IShippingStrategy
    {
        public void ScheduleShipping()
        {
            Console.WriteLine("DHL Shipping has been scheduled");
        }
    }

    public class FedExShippingStrategy : IShippingStrategy
    {
        public void ScheduleShipping()
        {
            Console.WriteLine("FedEx Shipping has been scheduled");
        }
    }

    public class InHouseShippingStrategy : IShippingStrategy
    {
        public void ScheduleShipping()
        {
            Console.WriteLine("In House Shipping has been scheduled");
        }
    }

    //这里和null object pattern相关
    #region 比较一般的写法
    //public class InvalidShippingStrategy : IShippingStrategy
    //{
    //    public void ScheduleShipping()
    //    {
    //        Console.WriteLine("Invalid Shiping Strategy");
    //    }
    //} 
    #endregion

    #region 更好的写法
    public class InvalidShippingStrategy : IShippingStrategy
    {
        private static readonly InvalidShippingStrategy instance = null;

        static InvalidShippingStrategy()
        {
            instance = new InvalidShippingStrategy();
        }

        public static InvalidShippingStrategy Instance
        {
            get
            {
                return instance;
            }
        }

        public void ScheduleShipping()
        {
            Console.WriteLine("Invalid Shipping Strategy");
        }
    }
    #endregion


    public class OrderProcessor
    {
        //这里体现null object pattern
        //public void ProcessOrder(IShippingStrategy shippingStrategy=null) //没用null object pattern之前的写法
        public void ProcessOrder(IShippingStrategy shippingStrategy)
        {
            #region 没用null object pattern之前的写法
            //if (shippingStrategy != null)
            //{
            //    shippingStrategy.ScheduleShipping();
            //}
            //else
            //{
            //    Console.WriteLine("Invalid Shipping Strategy");
            //} 
            #endregion

            shippingStrategy.ScheduleShipping();
        }
    }


    #endregion
}