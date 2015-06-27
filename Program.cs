using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("Hello World");
            var a = new List<int> { 10 };
            Console.WriteLine(a.Count);
            Test(a);
            Console.WriteLine(a.Count);
        }
        
        public static void Test(List<int> test) {
            test.Add(5);
        }
    }
}
