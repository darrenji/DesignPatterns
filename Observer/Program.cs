using System;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 使用event关键字
            //var person = new Person();
            //person.FallIll += CallDoctor;
            //person.CachACold();
            #endregion

            #region weak event pattern
            //var button = new Button();
            //var window = new Windows(button);
            //var windowRef = new WeakReference(window);
            //button.Fire();

            //Console.WriteLine("setting window to null");
            //window = null;

            //FireGC();
            //Console.WriteLine($"is the window alive after gc?{windowRef.IsAlive}");

            #endregion

            #region observable properties and sequences

            #endregion
        }

        //3、在运行时，给委托赋值
        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"a doctor must go {eventArgs.Address} to give service");
        }

        private static void FireGC()
        {
            Console.WriteLine("starging gc");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("gc is done");
        }
    }

    #region 使用event关键字
    public class Person
    {
        //1、定义事件
        public event EventHandler<FallsIllEventArgs> FallIll;

        //2、设置触发事件的方法
        public void CachACold()
        {
            FallIll?.Invoke(this, new FallsIllEventArgs { Address="from qingdao"});
        }
    }

    public class FallsIllEventArgs
    {
        public string Address;
    }
    #endregion

    #region weak event pattern
    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Windows
    {
        public Windows(Button button)
        {
            //button.Clicked += ButtonOnClicked;

            //WeakEventManager不存在
            //WeakEventManager<Button, EventArgs>.AddHandler(button, "Clicked", ButtonOnClicked);
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Button clicked (window handler)");
        }

        ~Windows()
        {
            Console.WriteLine("Window finalized");
        }
    }
    #endregion

    #region observable properties and sequences

    #endregion
}