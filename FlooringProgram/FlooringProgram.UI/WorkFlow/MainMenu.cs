using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.UI.WorkFlow
{
    public class MainMenu
    {
        public void Execute()
        {
            string input = "";
            do
            {
                Console.Clear();
                Console.WriteLine("*************************************************************");
                Console.WriteLine("*\t\tFlooring Program");
                Console.WriteLine("*");
                Console.WriteLine("* 1. Display Orders");
                Console.WriteLine("* 2. Add an Order");
                Console.WriteLine("* 3. Edit an Order");
                Console.WriteLine("* 4. Remove an Order");
                Console.WriteLine("* 5. Quit");
                Console.WriteLine("*");
                Console.WriteLine("*************************************************************");
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Enter Choice : ");

                input = Console.ReadLine();

                if (input != "5")
                {
                    ProcessChoice(input);
                }

<<<<<<< HEAD
            } while (input.ToUpper() != "Q" && input != "5" && input.ToUpper() != "QUIT");
=======
            } while (input != "5");
>>>>>>> de19b8767f0658dca15e69d01a64a695ff877416
        }

        private void ProcessChoice(string choice)
        {
            switch (choice.ToUpper())
            {
                case "1":

                case "2":
                    Console.WriteLine("Under construction :(");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    break;

                case "3":
                    Console.WriteLine("Under construction :(");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    break;

                case "4":
                    Console.WriteLine("Under construction :(");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    break;

                //case "5":
                //case "Q":
                //case "QUIT":
                //case "EXIT":
                //    Console.WriteLine("Under construction :(");
                //    Console.WriteLine("Press enter to continue");
                //    Console.ReadLine();
                //    break;

                default:
                    Console.WriteLine("{0} is an invalid entry", choice);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
