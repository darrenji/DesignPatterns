using Newtonsoft.Json;
using System;
using System.IO;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            var person1 = new Person(new[] { "darren", "ji" }, new Address("qingdao", 100));
            var person2 = person1.DeepCopy();
            person2.Names[0] = "sunny";
            person2.Address.StreetName = "jinan";
            person2.Address.StreetNo = 999;

            Console.WriteLine(person1.ToString());
            Console.WriteLine(person2.ToString());

        }
    }

    #region 使用ICloneable实现深度拷贝
    //实现ICloneable接口就可以.net core中已没有这个接口
    #endregion

    #region 使用BinaryFormatter序列化实现深度拷贝

    //[Seiralizable] //在.net core中没有
    //public class Person 
    //{
    //    public string[] Names;
    //    public Address Address;

    //    public Person(string[] names, Address address)
    //    {
    //        Names = names ?? throw new ArgumentNullException(paramName: nameof(names));
    //        Address = address ?? throw new ArgumentNullException(paramName: nameof(address));
    //    }
    //}

    //public class Address
    //{
    //    public string StreetName;
    //    public int StreetNo;

    //    public Address(string streetName, int streetNo)
    //    {
    //        StreetName = streetName ?? throw new ArgumentNullException(paramName: nameof(streetName));
    //        StreetNo = streetNo;
    //    }
    //}

    //public static class MyExtensions
    //{
    //    public static T DeepCopy<T>(this T self)
    //    {
    //        using (var ms = new MemoryStream())
    //        {
    //            //BinaryFormatter，在.net core中没有
    //            var formatter = new BinaryFormatter();
    //            formatter.Serialize(ms, self);
    //            ms.Seek(0, SeekOrigin.Begin);
    //            object obj = formatter.Deserialize(ms);
    //            return (T)obj;
    //        }
    //    }
    //}
    #endregion

    #region 使用Newtonsoft实现深度拷贝
    public class Person
    {
        public string[] Names;
        public Address Address;


        public Person()
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names ?? throw new ArgumentNullException(paramName: nameof(names));
            Address = address ?? throw new ArgumentNullException(paramName: nameof(address));
        }

        public override string ToString()
        {
            return $"{Names[0]}-{Names[1]}-{Address.StreetName}-{Address.StreetNo}";
        }
    }

    public class Address
    {
        public string StreetName;
        public int StreetNo;

        public Address()
        {

        }

        public Address(string streetName, int streetNo)
        {
            StreetName = streetName ?? throw new ArgumentNullException(paramName: nameof(streetName));
            StreetNo = streetNo;
        }
    }

    public static class MyExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            if (Object.ReferenceEquals(self, null))
                return default(T);

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(self), deserializeSettings);
        }
    }
    #endregion
}