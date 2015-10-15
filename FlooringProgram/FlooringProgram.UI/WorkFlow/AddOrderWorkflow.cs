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
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            //int orderDate = GetDateFromUser();

            //string customerName = GetCustomerNameFromUser();
            //string state = GetStateFromUser();
            //string productType = GetProductTypeFromUser();
            //decimal area = GetAreaFromUser();
            int orderNumber;
            decimal taxRate;
            decimal costPerSqFt;
            decimal laborPerSqFt;
            decimal materialCost;
            decimal laborCost;
            decimal tax;
            decimal total;

            DisplayOrderInfo();

            //ask user if they want to save
        }

        public string GetDateFromUser()
        {
            string input = "";
            do
            {
                string inputContinue = "";
                var date = DateTime.Now;
                Console.Clear();
                string orderDateString = "";
                DateTime orderDate;
                Console.Write("Enter an order date: ");
                orderDateString = Console.ReadLine();
                bool validDate = DateTime.TryParse(orderDateString, out orderDate);
                if (validDate)
                {
                    if (orderDate.ToString("MMddyyyy") == date.ToString("MMddyyyy"))
                        return orderDate.ToString("MMddyyyy");
                    if (orderDate.ToString("MMddyyyy") != date.ToString("MMddyyyy"))
                    {
                        Console.Write("That is not today's date; would you like to continue (y/n) ? : ");
                        inputContinue = Console.ReadLine();
                        if (inputContinue.ToUpper() == "Y")
                            return orderDate.ToString("MMddyyyy");
                        if (inputContinue.ToUpper() == "N")
                            return "X";
                    }
                }
                if (!validDate)
                    Console.WriteLine("That does not look like a valid date...");
                Console.WriteLine("Press enter to continue, or (M)ain Menu...");
                input = Console.ReadLine().ToUpper();
                if (input.ToUpper() == "M")
                {
                    return "X";
                }
            } while (true);

        }

        //Console.WriteLine("Please Enter a valid date in MMDDYY format (no other characters)");
        //Console.WriteLine("Press enter to continue");
        //Console.ReadLine();




        public string GetCustomerNameFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter customer name : ");
                string customerName = Console.ReadLine();

                if (customerName != "" && customerName.Length >= 2)
                {
                    return customerName;
                }

                Console.WriteLine("Please Enter a valid customer name (customer names must be at least two characters).");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public string GetStateFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please choose a state (OH, PA, MI, or IN) : ");
                string state = Console.ReadLine().ToUpper();

                switch (state)
                {
                    case "OH":
                    case "PA":
                    case "MI":
                    case "IN":
                        return state;
                    default:
                        Console.WriteLine("Please enter a valid state abbreviation (from the list provided!)");
                        Console.WriteLine("Press enter to continue");
                        Console.ReadLine();
                        break;
                }

            } while (true);
        }

        public string GetProductTypeFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter the product type (Carpet, Laminate, Tile, Wood) : ");
                string productType = Console.ReadLine();

                switch (productType.ToUpper())
                {
                    case "CARPET":


                    case "LAMINATE":
                    case "TILE":
                    case "WOOD":
                        return productType;
                    default:
                        Console.WriteLine("Please choose a valid product type");
                        Console.WriteLine("Press enter to continue");
                        Console.ReadLine();
                        break;

                }



            } while (true);
        }

        public decimal GetAreaFromUser()
        {
            do
            {
                decimal area;
                Console.Clear();
                Console.Write("Please enter the area needed (sq ft) : ");
                if (decimal.TryParse((Console.ReadLine()), out area) && area > 0)
                {
                    return area;
                }

                Console.WriteLine("Please Enter a valid area value");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public void DisplayOrderInfo()
        {
            string orderDate = GetDateFromUser();
            if (orderDate != "X")

            {
                string customerName = "";
                customerName = GetCustomerNameFromUser();
                string state = GetStateFromUser();
                string productType = "";
                productType = GetProductTypeFromUser();
                decimal area = 0;
                area = GetAreaFromUser();

                var ops = new OrderOperations();

                var response = ops.AddOrder(orderDate, customerName, state, productType, area);

                if (response.Success)
                {
                    Console.Clear();
                    Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.Substring(0, 2),
                        response.Order.OrderDate.Substring(2, 2),
                        response.Order.OrderDate.Substring(4));
                    Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                    Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                    Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                    Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                    Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                    Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                    Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                    Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                    Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate/100,
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
                    Console.WriteLine("There was an error");
                    Console.WriteLine("Press enter to return to main menu");
                    Console.ReadLine();
                }
            }
        }

        public void ConfirmUserCommit(Response OrderInfo)
        {
            Console.Write("Do you want to commit these changes to memory (y/n) ? : ");
            string input = Console.ReadLine();

            if (input.ToUpper() == "Y" || input.ToUpper() == "YES")
            {
                var ops = new OrderOperations();

                var UIOrder = new Response();
                UIOrder = OrderInfo;

                // this method will pass order info to the BLL
                ops.PassAddToData(UIOrder);
                Console.WriteLine("Your order has been saved successfully!");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Changes not saved!");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }


        }
    }
}
