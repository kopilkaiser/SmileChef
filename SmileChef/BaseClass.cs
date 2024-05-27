namespace SmileChef
{
    public class BaseClass
    {
        public virtual void Display()
        {
            Console.WriteLine("BaseClass Display method");
        }
    }

    public class DerivedClass : BaseClass
    {
        public override void Display()
        {
            Console.WriteLine("DerivedClass Display method");
        }
    }

    public class AnotherDerivedClass : BaseClass
    {
        public override void Display()
        {
            Console.WriteLine("AnotherDerivedClass Display method");
        }
    }
}
