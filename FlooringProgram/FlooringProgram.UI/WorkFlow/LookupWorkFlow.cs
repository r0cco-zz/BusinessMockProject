using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Models;
using System.IO;

namespace FlooringProgram.UI.WorkFlow
{
    public class LookupWorkFlow
    {
        private List<Order> _orderListFromDate;

        public void Execute()
        {
            int orderDate = GetOrderDateFromUser();
            DisplayOrdersFromDate(orderDate);
        }

        public int GetOrderDateFromUser()
        {
            do
            {
                Console.Clear();
                string orderDateString;
                int orderDate;
                Console.Write("Enter an date in MMDDYYYY format (no other characters) : ");
                orderDateString = Console.ReadLine();
                bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDateString));
                if (int.TryParse(orderDateString, out orderDate) && orderDate.ToString().Length == 8
                    && doesExist)
                {
                    return orderDate;
                }

                Console.WriteLine("That was not a valid account number.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public void DisplayOrdersFromDate(int orderDate)
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
                Console.WriteLine("Order Information");
                Console.WriteLine("\t{0}, {1}", order.CustomerName, order.Total);
            }

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
