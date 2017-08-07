using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            var bank = new BankAccount();
            var commands = new List<BankAccountCommand> {
                new BankAccountCommand(bank, BankAccountCommand.Action.Deposit,100),
                new BankAccountCommand(bank, BankAccountCommand.Action.Withdraw,1000)
            };
            Console.WriteLine(bank);

            foreach (var c in commands)
                c.Call();
            Console.WriteLine(bank);

            foreach (var c in Enumerable.Reverse(commands))
                c.Undo();
            Console.WriteLine(bank);
        }
    }

    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"deposited {amount},balance is {balance}");
        }

        public bool Withdraw(int amount)
        {
            if(balance-amount >= overdraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"withdraw {amount}, balance is {balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"balance is {balance}";
        }

    }

    public interface ICommand
    {
        void Call();
        void Undo();
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount account;
        private int amount;
        private Action action;
        private bool succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account ?? throw new ArgumentNullException(paramName: nameof(account));
            this.action = action;
            this.amount = amount;
        }

        public enum Action
        {
            Deposit, Withdraw
        }
        public void Call()
        {
            switch(action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    succeeded = true;
                    break;
                case Action.Withdraw:
                    succeeded = account.Withdraw(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
               
        }

        public void Undo()
        {
            if (!succeeded) return;
            switch(action)
            {
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}