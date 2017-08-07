using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            #region Protection Proxy
            //ICar car = new CarProxy(new Driver(16));
            //car.Drive();
            #endregion

            #region Property Proxy
            //var c = new Creature();
            //c.Agility = 10;
            //c.Agility = 10;
            #endregion

            #region Dynamic Proxy for Logging
            //var bank = new BankAccount();
            //bank.Deposit(100);
            //bank.Deposit(50);
            //Console.WriteLine(bank.ToString());
            #endregion

            #region Castle DynamicProxy
            Person p = Freezable.MakeFreezable<Person>();
            p.FirstName = "Foo";
            p.LastName = "Bar";
            Console.WriteLine(p);
            Freezable.Freeze(p);
            p.FirstName = "what";
            Console.WriteLine(p);
            #endregion
        }
    }

    #region Protection Proxy
    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("汽车正在行驶");
        }
    }

    //来一个有关Car的代理，这个代理有ICar相同的接口，但可以有额外的功能
    //怎么和装饰器模式很像呢？
    public class CarProxy : ICar
    {
        private Driver _driver;
        Car _car = new Car();

        public CarProxy(Driver driver)
        {
            _driver = driver;       
        }
        public void Drive()
        {
            if(_driver.Age>18)
            {
                _car.Drive();
            }
            Console.WriteLine("不满18岁，不能驾驶");
        }
    }

    public class Driver
    {
        public int Age;

        public Driver(int age)
        {
            Age = age;
        }
    }
    #endregion

    #region Property Proxy
    public class MyProperty<T> where T : new()
    {
        private T _value;

        public MyProperty() : this(default(T))
        {

        }


        public MyProperty(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(this._value, Value)) return;
                Console.WriteLine($"正在给{_value}属性赋值");
                _value = Value;
            }
        }

        public static implicit operator T(MyProperty<T>  property)
        {
            return property._value;
        }

        public static implicit operator MyProperty<T>(T value)
        {
            return new MyProperty<T>(value);
        }

        public bool Equals(MyProperty<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(this._value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;
            return Equals((MyProperty<T>)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(MyProperty<T> left, MyProperty<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MyProperty<T> left, MyProperty<T> right)
        {
            return !Equals(left, right);
        }
    }

    public class Creature
    {
        private MyProperty<int> _agility = new MyProperty<int>();

        public int Agility
        {
            get => _agility.Value;
            set => _agility.Value = value;
        }
    }
    #endregion

    #region Dynamic Proxy for Logging
    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private int _balance;
        private int _overdraftlimit = -500;

        public void Deposit(int amount)
        {
            _balance += amount;
            Console.WriteLine($"您的账户存入{amount}元，当前余额为{_balance}");
        }

        public bool Withdraw(int amount)
        {
            if(_balance - amount >= _overdraftlimit)
            {
                _balance -= amount;
                Console.WriteLine($"您的账户取出{amount}元，当前余额是{_balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"nameof(_balance):{_balance}";
        }
    }
    #endregion

    #region Castle DynamicProxy
    //运行时生成的对象对原先对象的方法进行拦截
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }

    public static class Freezable 
    {
        private static readonly IDictionary<object, IFreezable> _freezables = new Dictionary<object, IFreezable>();
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static bool IsFreezable(object obj)
        {
            return obj != null && _freezables.ContainsKey(obj);
        }

        public static void Freeze(object freezable)
        {
            if(!IsFreezable(freezable))
            {
                throw new Exception("not freeable object:" + freezable.GetHashCode());
            }
            _freezables[freezable].Freeze();
        }

        public static bool IsFrozen(object freezable)
        {
            return IsFreezable(freezable) && _freezables[freezable].IsFrozen;
        }

        public static TFreezable MakeFreezable<TFreezable>() where TFreezable:class, new()
        {
            var freezableInterceptor = new FreezableInterceptor();
            var proxy = _generator.CreateClassProxy<TFreezable>(new CallLoggingInterceptor(),freezableInterceptor);
            _freezables.Add(proxy, freezableInterceptor);
            return proxy;
        }
    }

    //拦截器
    public class FreezableInterceptor : IInterceptor, IFreezable
    {
        private bool _isFrozen;

        public bool IsFrozen => _isFrozen;

        public void Freeze()
        {
            _isFrozen = true;
        }

        public void Intercept(IInvocation invocation)
        {
            if(_isFrozen && invocation.Method.Name.StartsWith("set_",StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("ObjectFrozenException");
            }
            invocation.Proceed();
        }
    }

    public class CallLoggingInterceptor : IInterceptor
    {
        private int indentation;

        public void Intercept(IInvocation invocation)
        {
            try
            {
                indentation++;
                Console.WriteLine($"{indentation} ! {invocation.Method.Name}");
            }
            finally
            {
                indentation--;
            }
        }
    }

    public class Person
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
    #endregion
}