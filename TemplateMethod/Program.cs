using System;

namespace TemplateMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var chess = new Chess();
            chess.Run();
            Console.WriteLine();
        }
    }


    public abstract class Game
    {
        //这里体现了Template Method
        public void Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();
            Console.WriteLine($"Player {WinningPlayer} wins.");
        }

        protected int currentPlayer;
        protected readonly int numberOfPlayers;

        protected Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }

        protected abstract void Start();
        protected abstract void TakeTurn();
        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }
    }

    public class Chess : Game
    {
        private int turn = 1;
        private int maxTurns = 10;

        public Chess():base(2)
        {

        }
        protected override bool HaveWinner => turn == maxTurns;

        protected override int WinningPlayer => currentPlayer;

        protected override void Start()
        {
            Console.WriteLine($"starting a game of chess with {numberOfPlayers}");
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"turn {turn++} taken by player{currentPlayer}");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }
    }
}