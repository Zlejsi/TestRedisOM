using System;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{


    public class IntID
    {
        public int Id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IntID id = new IntID();
            Type type = id.GetType();

            PropertyInfo info = type.GetProperties().First(x => x.Name == "Id");
            info.SetValue(id,(object)"2");
            Console.WriteLine("Hello World!");
        }
    }
}
