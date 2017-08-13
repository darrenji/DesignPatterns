using System;
using System.Collections.Generic;

namespace Visitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var animals = new List<BaseAnimal>() {
                new CatAnimal(),
                new DogAnimal()
            };

            var vistor = new CollectStepsAnimalVisitor();
            vistor.Dispatch(animals);//让每一个接受vistor

            var allSteps = vistor.Steps;
            foreach(var step in allSteps)
            {
                Console.WriteLine("STEP: " +step);
            }
        }
    }

    public abstract class BaseAnimal
    {
        //接受参观者,让visitor做事情
        public abstract void Accept(IAnimalVistor visitor);
    }

    public class DogAnimal : BaseAnimal
    {
        public string MoveHeadStepName = "Dog move head";
        public string WagTailStepName = "Dog wag tail";
        public string OpenMouthStepName = "Dog open mouth";
        public string CloseMouthStepName = "Dog close mouth";

        public override void Accept(IAnimalVistor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CatAnimal : BaseAnimal
    {
        public string OpenMouseStepName = "Cat open mouth";
        public string CloseMouthStepName = "Cat close mouth";

        public override void Accept(IAnimalVistor visitor)
        {
            visitor.Visit(this);
        }
    }

    //这里体现了Vistor
    public interface IAnimalVistor
    {
        void Dispatch<T>(IEnumerable<T> animals) where T : BaseAnimal;

        //定义visit哪个对象
        void Visit(DogAnimal animal);
        void Visit(CatAnimal animal);
    }

    public abstract class AbstractAnimalVisitor : IAnimalVistor
    {
        public void Dispatch<T>(IEnumerable<T> animals) where T : BaseAnimal
        {
            foreach (var animal in animals)
                animal.Accept(this);
        }

        public abstract void Visit(DogAnimal animal);
        public abstract void Visit(CatAnimal animal);
    }

    //所有visitor要做的事情，都定义在了这里
    public class CollectStepsAnimalVisitor : AbstractAnimalVisitor
    {
        public List<string> Steps = new List<string>();

        public override void Visit(DogAnimal animal)
        {
            Steps.Add(animal.MoveHeadStepName);
            Steps.Add(animal.WagTailStepName);
            Steps.Add(animal.OpenMouthStepName);
            Steps.Add(animal.CloseMouthStepName);
        }

        public override void Visit(CatAnimal animal)
        {
            Steps.Add(animal.OpenMouseStepName);
            Steps.Add(animal.CloseMouthStepName);
        }
    }
}