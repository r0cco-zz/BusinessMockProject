using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;

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

        public int GetDateFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter order date (as MMDDYYYY) : ");
                int orderDate;


                if (int.TryParse((Console.ReadLine()), out orderDate) && orderDate.ToString().Length == 8)
                {
                    return orderDate;
                }

                Console.WriteLine("Please Enter customer name (customer names must be at least two characters).");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

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

                Console.WriteLine("Please Enter customer name (customer names must be at least two characters).");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public string GetStateFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter the state (2 letter abbr.) where the customer is located : ");
                string state = Console.ReadLine();

                if (state != "" && state.Length == 2)
                {
                    return state;
                }

                Console.WriteLine("Please Enter the state the customer is located.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public string GetProductTypeFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter the product type : ");
                string productType = Console.ReadLine();

                if (productType != "")
                {
                    return productType;
                }

                Console.WriteLine("Please Enter a valid product type");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public decimal GetAreaFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Please enter the area needed : ");
                decimal area = decimal.Parse(Console.ReadLine());

                if (area > 0)
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
            int orderDate = GetDateFromUser();
            string customerName = "";
            customerName = GetCustomerNameFromUser();
            string state = GetStateFromUser();
            string productType = GetProductTypeFromUser();
            decimal area = GetAreaFromUser();

            var ops = new OrderOperations();

            var response = ops.AddOrder(orderDate, customerName, state, productType, area);

            if (response.Success)
            {
                Console.WriteLine(
                    "Name : {0}\nState : {1}\nOrder Number : {2}\nArea : {3}\nProduct type : {4}\nMaterial cost per sq ft : {5}\nLabor cost per sq ft : {6}\nState Tax : {7}\nTotal Material Cost : {8}\nTotal Labor Cost : {9}\nTotal Tax : {10}\nTotal Cost : {11}",
                    response.Order.CustomerName, response.Order.State, response.Order.OrderNumber, response.Order.Area,
                    response.Order.ProductType.ProductType, response.Order.ProductType.MaterialCost,
                    response.Order.ProductType.LaborCost, response.Order.TaxRate, response.Order.MaterialCost,
                    response.Order.LaborCost, response.Order.Tax, response.Order.Total);

                Console.ReadLine();
            }
        }
    }
}
