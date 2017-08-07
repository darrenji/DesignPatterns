using System;

namespace ChainOfResponsibility
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Method Chain
            //var goblin = new Creature("Goblin", 2, 3);
            //Console.WriteLine(goblin);

            //var root = new CreatureModifier(goblin);

            ////加上以下代码就不会执行
            ////root.Add(new NoBonusesModifier(goblin));

            //Console.WriteLine("let's double the goblin's attack");
            //root.Add(new DoubleAttackModifier(goblin));

            //Console.WriteLine("let's increase goblin's defense");
            //root.Add(new IncreaseDefenseModifier(goblin));

            //root.Handle();
            //Console.WriteLine(goblin);
            #endregion

            #region Broker Chain
            var game = new Game();
            var goblin = new Creature1(game, "string goblin", 3, 3);
            Console.WriteLine(goblin);

            using (new DoubleAttackModifier1(game, goblin))
            {
                Console.WriteLine(goblin);
                using (new IncreaeDefenseModifier1(game, goblin))
                {
                    Console.WriteLine(goblin);
                }
            }

            Console.WriteLine(goblin);
            #endregion
        }
    }

    #region Method Chain
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"name:{Name}, attack:{Attack}, defense:{Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; //linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
        }

        //通常这里的CreatureModifier是子类
        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        //在基类中设置一个虚的要执行的方法
        public virtual void Handle() => next?.Handle();

    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {

        }

        //在子类中真正执行
        public override void Handle()
        {
            Console.WriteLine($"doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {

        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    //终止
    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {

        }

        public override void Handle()
        {
           //do nothing
        }
    }
    #endregion

    #region Broker Chain

    //这个类就是Broker,负责执行动作
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerfomQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    //负责传递信息
    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defense
        }
        public Argument WhatToQuery;

        //给外界设定的值
        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(paramName: nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }

    }

    //组织完Query的信息后，交给Broker触发事件
    public class Creature1
    {
        private Game game;
        public string Name;
        private int attack, defense;

        public Creature1(Game game, string name, int attack, int defense)
        {
            this.game = game ?? throw new ArgumentNullException(paramName: nameof(game));
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            this.attack = attack;
            this.defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerfomQuery(this, q);
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, attack);
                game.PerfomQuery(this, q);
                return q.Value;
            }
        }

        public override string ToString()
        {
            return $"Name:{Name},Attack:{Attack},Defense:{Defense}";
        }
    }

    public abstract class CreatureModifier1 : IDisposable
    {
        protected Game game;
        protected Creature1 creature;

        protected CreatureModifier1(Game game, Creature1 creature)
        {
            this.game = game ?? throw new ArgumentNullException(paramName: nameof(game));
            this.creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier1 : CreatureModifier1
    {
        public DoubleAttackModifier1(Game game, Creature1 creature) : base(game, creature)
        {

        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Attack)
                q.Value *= 2;
        }
    }

    public class IncreaeDefenseModifier1 : CreatureModifier1
    {
        public IncreaeDefenseModifier1(Game game, Creature1 creature) : base(game, creature)
        {
          

        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Defense)
                q.Value += 2;
        }
    }

    #endregion
}