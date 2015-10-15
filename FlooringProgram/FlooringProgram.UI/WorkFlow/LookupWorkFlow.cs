using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Models;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace FlooringProgram.UI.WorkFlow
{
    public class LookupWorkFlow
    {
        private List<Order> _orderListFromDate;

        public void Execute()
        {
            string orderDate = GetOrderDateFromUser();
            if (orderDate != "X")
            {
                DisplayOrdersFromDate(orderDate);
            }
           
        }

        public string GetOrderDateFromUser()
        {
            string input = "";
            do
            {
                Console.Clear();
                string orderDateString = "";
                DateTime orderDate;
                Console.Write("Enter an order date: ");
                orderDateString = Console.ReadLine();
                bool validDate = DateTime.TryParse(orderDateString, out orderDate);
                bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDate.ToString("MMddyyyy")));
                if (doesExist && validDate)
                {
                    return orderDate.ToString("MMddyyyy");
                }
                if (!validDate)
                    Console.WriteLine("That does not look like a valid date...");
                if (validDate && !doesExist)
                Console.WriteLine("There are no matching orders...");
                Console.WriteLine("Press enter to continue, or (M)ain Menu...");
                input = Console.ReadLine().ToUpper();
                if (input.ToUpper() == "M")
                {
                    return "X";
                }
            } while (true);
            
        }
        

        public void DisplayOrdersFromDate(string orderDate)
        {
            var ops = new OrderOperations();
            var response = ops.GetAllOrdersFromDate(orderDate);

            if (response.Success)
            {
                _orderListFromDate = response.OrderList;
                PrintOrderInfo(response.OrderList);
            }
            else
            {
                Console.WriteLine("Date not found.");
            }
        }

        public void PrintOrderInfo(List<Order> orderList)
        {
            foreach (var order in orderList)
            {
                Console.WriteLine("Order number {0}", order.OrderNumber);
                Console.WriteLine("\t{0}, Total : {1:c}\n", order.CustomerName, order.Total);
            }

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
