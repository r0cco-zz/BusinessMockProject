﻿using System;
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

            string orderDateRemove = GetOrderDateFromUser();
            DisplayAllOrdersFromDate(orderDateRemove);
            int orderNumberRemove = GetOrderNumberFromUser();
            var ops = new OrderOperations();
            var response = ops.RemoveOrder(orderDateRemove, orderNumberRemove);
            DisplayOrder(response);
        }

        public string GetOrderDateFromUser()
        {
            do
            {
                Console.Clear();
                string orderDateString;
                DateTime orderDate;
                Console.Write("Enter an order date : ");
                orderDateString = Console.ReadLine();
                bool validDate = DateTime.TryParse(orderDateString, out orderDate);
                bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDate.ToString("MMddyyyy")));
                if (validDate && doesExist)
                {
                    return orderDate.ToString("MMddyyyy");
                }
                if (validDate && !doesExist)
                {
                    var log = new ErrorLogger()
                    {
                        TimeOfError = DateTime.Now,
                        Message = String.Format("RemoveOrder : no orders found on date entered : {0}", orderDateString)
                    };
                    var ops = new OrderOperations();
                    ops.ErrorPassdown(log);

                    Console.WriteLine("There are no orders for that date...");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }
                if (!validDate)
                {
                    var log = new ErrorLogger()
                    {
                        TimeOfError = DateTime.Now,
                        Message = String.Format("RemoveOrder : invalid date entered : {0}", orderDateString)
                    };
                    var ops = new OrderOperations();
                    ops.ErrorPassdown(log);

                    Console.WriteLine("Please enter a valid date...");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }
                

            } while (true);
        }

        public int GetOrderNumberFromUser()
        {
            do
            {
                string orderNumberString;
                int orderNumber;
                Console.Write("Enter an order number to delete : ");
                orderNumberString = Console.ReadLine();
                bool validInput = int.TryParse(orderNumberString, out orderNumber);
                if (validInput)
                {
                    return orderNumber;
                }

                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = String.Format("RemoveOrder : invalid order number entered : {0}", orderNumberString)
                };
                var ops = new OrderOperations();
                ops.ErrorPassdown(log);

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
                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = String.Format("RemoveOrder : order does not exist on selected day : {0}", response.Message)
                };
                var ops = new OrderOperations();
                ops.ErrorPassdown(log);

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

        public void DisplayAllOrdersFromDate(string orderDate)
        {
            OrderOperations ops = new OrderOperations();
            var response = ops.GetAllOrdersFromDate(orderDate);

            if (response.Success)
            {
                foreach (var order in response.OrderList)
                {
                    Console.WriteLine("Order number {0}", order.OrderNumber);
                    Console.WriteLine("\t{0}, Total : {1:c}\n", order.CustomerName, order.Total);
                }


            }
            if (!response.Success)
            {
                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = String.Format("RemoveOrder : error displaying orders from selected date : {0}", response.Message)
                };
                var ops2 = new OrderOperations();
                ops2.ErrorPassdown(log);

                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }
    }
}
