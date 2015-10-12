using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Models;

namespace FlooringProgram.UI.WorkFlow
{
    public class LookupWorkFlow
    {
        private Order _currentOrder;

        public void Execute()
        {
            string orderDate = GetOrderDateFromUser();
            DisplayOrdersFromDate(orderDate);
        }

        public string GetOrderDateFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Enter an date in MMDDYYYY format : ");
                string input = Console.ReadLine();

                if (input.Length == 8)
                {
                    return input;
                }

                Console.WriteLine("That was not a valid account number.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public void DisplayOrdersFromDate(string orderDate)
        {
            var ops = new OrderOperations();
            var response = ops.GetOrder(orderDate);

            if (response.Success)
            {
                _currentOrder = response.OrderInfo;
                PrintOrderInfo(response.OrderInfo);
            }
            else
            {
                Console.WriteLine("Date not found.");
            }
        }

        public void PrintOrderInfo(Order orderInfo)
        {
            Console.Clear();
            Console.WriteLine("Order Information");
            Console.WriteLine("****************************************");
            Console.WriteLine();
            Console.WriteLine("Order information : {0}", orderInfo.OrderNumber);

        }
    }
}
