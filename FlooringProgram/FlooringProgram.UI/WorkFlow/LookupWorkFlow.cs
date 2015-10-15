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
                PromptUserForMoreDeets(orderDate);
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
        

        //public void DisplayOrdersFromDate(int orderDate)


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
        }

        public void PromptUserForMoreDeets(string orderDate)
        {
            var ops = new OrderOperations();

            var response = ops.GetAllOrdersFromDate(orderDate);

            bool notValidInput;
            do
            {
                Console.Write("Enter an order number to see more detail (or enter 'm' to return to main menu) : ");
                string input = Console.ReadLine();
                int orderNo;
                if (int.TryParse(input, out orderNo) &&
                    File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDate)))
                {
                    Console.Clear();
                    var order = from a in response.OrderList
                                where a.OrderNumber == orderNo
                                select a;
                    foreach (var toPrint in order)
                    {
                        Console.WriteLine("Order date : {0}/{1}/{2}", toPrint.OrderDate.ToString().Substring(0, 2),
                            toPrint.OrderDate.ToString().Substring(2, 2),
                            toPrint.OrderDate.ToString().Substring(4));
                        Console.WriteLine("Order number {0:0000}", toPrint.OrderNumber);
                        Console.WriteLine("\nCustomer name : {0}", toPrint.CustomerName);
                        Console.WriteLine("Area : {0} sq ft", toPrint.Area);
                        Console.WriteLine("Product type : {0}", toPrint.ProductType.ProductType);
                        Console.WriteLine("Material cost per sq ft : {0:c}", toPrint.ProductType.MaterialCost);
                        Console.WriteLine("Labor cost per sq ft : {0:c}", toPrint.ProductType.LaborCost);
                        Console.WriteLine("Total material cost : {0:c}", toPrint.MaterialCost);
                        Console.WriteLine("Total labor cost : {0:c}", toPrint.LaborCost);
                        Console.WriteLine("{0} state tax ({1:p}) : {2:c}", toPrint.State, toPrint.TaxRate / 100,
                            toPrint.Tax);
                        Console.WriteLine("\nOrder total : {0:c}", toPrint.Total);
                        Console.WriteLine();
                        Console.WriteLine("Press enter to return to main menu");
                        Console.ReadLine();
                    }
                    notValidInput = false;
                }
                else if (input.ToUpper() == "M")
                {
                    notValidInput = false;
                }
                else
                {
                    notValidInput = true;
                }
            } while (notValidInput);

        }
    }
}
