using System;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            IRenderer circleRender = new RenderCircle();
            Circle circle = new Circle(circleRender, 2);
            circle.ComputeArea();
            circle.Resize();
            circle.ComputeArea();
        }
    }

    public interface IRenderer
    {
        void RenderShape(float radius);
    }

    public class RenderCircle : IRenderer
    {
        public void RenderShape(float radius)
        {
            Console.WriteLine($"正在渲染面积为{radius*radius*3.14}的圆形");
        }
    }

    public class RenderSquare : IRenderer
    {
        public void RenderShape(float radius)
        {
            Console.WriteLine($"正在渲染面积为{radius * radius}的正方形");
        }
    }

    public abstract class Shape
    {
        protected IRenderer _renderer;

        protected Shape(IRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(paramName: nameof(renderer));
        }

        public abstract void ComputeArea();
        public abstract void Resize();
    }

    public class Circle : Shape
    {
        private  float _radius;

        public Circle(IRenderer render, float radius) : base(render)
        {
            _radius = radius;
        }

        public override void ComputeArea()
        {
            _renderer.RenderShape(_radius);
        }

        public override void Resize()
        {
           _radius= _radius * 2;
        }
    }
}