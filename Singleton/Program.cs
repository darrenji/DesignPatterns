using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            #region 最基本的Singleton
            //var db = SingletonDatabase.Instance;
            //Console.WriteLine(db.GetPopulation("Tokyo")); 
            #endregion


        }
    }

    #region 最基本的Singleton
    public interface IDatabase
    {
        int GetPopulation(string cityName);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        //私有实例字段+延迟加载
        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        //公共静态实例属性
        public static SingletonDatabase Instance = instance.Value;


        //私有构造函数
        private SingletonDatabase()
        {
            Console.WriteLine("数据库初始化中");

            capitals = File.ReadAllLines("capitals.txt")
                .Batch(2)
                .ToDictionary(
                    e => e.ElementAt(0).Trim(),
                    e => int.Parse(e.ElementAt(1))
                );
        }

        public int GetPopulation(string cityName)
        {
            return capitals[cityName];
        }
    }
    #endregion
}