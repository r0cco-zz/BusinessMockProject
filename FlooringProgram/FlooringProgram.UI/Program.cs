using System;
using FlooringProgram.UI.WorkFlow;

namespace FlooringProgram.UI
{
    class Program
    {
       

        static void Main(string[] args)
        {
            var menu = new MainMenu();
            menu.Execute();
            Console.WriteLine("Okay, goodbye...");
            Console.ReadLine();
        }
    }
}

