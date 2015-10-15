using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Models;

namespace FlooringProgram.UI.WorkFlow
{
    public class RemoveOrderWorkFlow
    {
        public void Execute()
        {
            int orderDateRemove = GetOrderDateFromUser();
            int orderNumberRemove = GetOrderNumberFromUser();
            var ops = new OrderOperations();
            var response = ops.RemoveOrder(orderDateRemove, orderNumberRemove);
            DisplayOrder(response);
        }

        public int GetOrderDateFromUser()
        {
            do
            {
                Console.Clear();
                string orderDateString;
                int orderDate;
                Console.Write("Enter a date in MMDDYYYY format (no other characters) : ");
                orderDateString = Console.ReadLine();
                if (int.TryParse(orderDateString, out orderDate) && orderDate.ToString().Length == 8)
                {
                    return orderDate;
                }

                Console.WriteLine("Please enter a date in MMDDYYYY format.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public int GetOrderNumberFromUser()
        {
            do
            {
                Console.Clear();
                string orderNumberString;
                int orderNumber;
                Console.Write("Enter an order number to search for : ");
                orderNumberString = Console.ReadLine();
                //bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDateString));
                if (int.TryParse(orderNumberString, out orderNumber))
                {
                    return orderNumber;
                }

                Console.WriteLine("Please enter a number to check for an order.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public void DisplayOrder(Response response)
        {
            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.ToString().Substring(0, 2),
                    response.Order.OrderDate.ToString().Substring(2, 2), response.Order.OrderDate.ToString().Substring(4));
                Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate / 100,
                    response.Order.Tax);
                Console.WriteLine("\nOrder total : {0:c}", response.Order.Total);
                Console.WriteLine();
                Console.WriteLine();

                ConfirmUserCommit(response);

                //method to take this input and use it to either write on the data or not
            }
            if (!response.Success)
            {
                Console.Clear();
                Console.WriteLine("There were no orders matching that data...");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }

        public void ConfirmUserCommit(Response OrderInfo)
        {
            Console.Write("Are you sure you would like to delete this order (y/n) ? : ");
            string input = Console.ReadLine();

            if (input.ToUpper() == "Y" || input.ToUpper() == "YES")
            {
                var ops = new OrderOperations();

                var UIOrder = new Response();
                UIOrder = OrderInfo;

                // this method will pass order info to the BLL
                ops.PassRemoveFromData(UIOrder);
                Console.WriteLine("Your order has been successfully removed!");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Order not removed!");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }


        }


    }
}
