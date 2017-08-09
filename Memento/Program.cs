using System;
using System.Collections.Generic;

namespace Memento
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1
            //var ba = new BankAccount(100);
            //var m1 = ba.Deposit(20);
            //var m2 = ba.Deposit(30);
            //Console.WriteLine(ba);//150

            //ba.Restore(m1);
            //Console.WriteLine(ba);//120

            //ba.Restore(m2);
            //Console.WriteLine(ba);//150 
            #endregion

            #region 2
            var ba = new BankAccount(100);
            ba.Deposit(50);
            ba.Deposit(25);
            Console.WriteLine(ba);

            ba.Undo();
            Console.WriteLine(ba);
            ba.Undo();
            Console.WriteLine(ba);
            ba.Redo();
            Console.WriteLine(ba);
            #endregion
        }
    }

    public class BankAccount
    {
        private int _balance;
        private List<Memento> changes = new List<Memento>();
        private int current; //当前Memento的索引位置

        public BankAccount(int balance)
        {
            this._balance = balance;
            changes.Add(new Memento(_balance));
        }

        //把每一次这种动作告诉Memento
        public Memento Deposit(int amount)
        {
            _balance += amount;
            var m = new Memento(_balance);
            changes.Add(m);
            ++current;
            return m;
        }

        public Memento Restore(Memento m)
        {
            if(m!=null)
            {
                _balance = m.Balance;
                changes.Add(m);
                return m;
            }
            return null;
        }

        public Memento Undo()
        {
            if(current>0)
            {
                var m = changes[--current];
                _balance = m.Balance;
                return m;
            }
            return null;
        }

        public Memento Redo()
        {
            if(current+1<changes.Count)
            {
                var m = changes[++current];
                _balance = m.Balance;
                return m;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{nameof(_balance)}:{_balance}";
        }
    }

    public class Memento
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance;
        }
    }
}