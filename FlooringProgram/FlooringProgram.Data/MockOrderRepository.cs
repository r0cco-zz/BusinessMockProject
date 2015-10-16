using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;

namespace FlooringProgram.Data
{
    public class MockOrderRepository : IOrderRepository
    {
        public List<Order> GetAllOrders(string orderDate)
        {
            string _filePath = @"DataFiles\Orders_12122054.txt"; 

            GetProducts();

            List<Order> orders = new List<Order>();

            //need to check if filepath is null!!
            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                if (reader[i].Length < 1)
                {
                    continue;
                }
                var columns = reader[i].Split(',');

                var order = new Order();
                order.ProductType = new ProductTypes();

                order.OrderNumber = int.Parse(columns[0]);

                for (int j = 1; j < columns.Count() - 11; j++)
                {
                    order.CustomerName += columns[j] + ",";
                }
                order.CustomerName += columns[columns.Count() - 11];

                order.State = columns[columns.Count() - 10];
                order.TaxRate = decimal.Parse(columns[columns.Count() - 9]);
                order.ProductType.ProductType = (columns[columns.Count() - 8]);
                order.Area = decimal.Parse(columns[columns.Count() - 7]);
                order.ProductType.MaterialCost = decimal.Parse(columns[columns.Count() - 6]);
                order.ProductType.LaborCost = decimal.Parse(columns[columns.Count() - 5]);
                order.MaterialCost = decimal.Parse(columns[columns.Count() - 4]);
                order.LaborCost = decimal.Parse(columns[columns.Count() - 3]);
                order.Tax = decimal.Parse(columns[columns.Count() - 2]);
                order.Total = decimal.Parse(columns[columns.Count() - 1]);

                order.OrderDate = (_filePath.Substring(17, 8));

                orders.Add(order);
            }

            return orders;
        }

        public List<ProductTypes> GetProducts()
        {

            string _filePath = @"DataFiles\Products.txt";


            List<ProductTypes> products = new List<ProductTypes>();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var product = new ProductTypes();

                product.ProductType = columns[0];
                product.MaterialCost = decimal.Parse(columns[1]);
                product.LaborCost = decimal.Parse(columns[2]);

                products.Add(product);
            }

            return products;
        }

        public ProductTypes GetProduct(string ProductType)
        {
            List<ProductTypes> products = GetProducts();
            return products.FirstOrDefault(a => a.ProductType == ProductType);
        }

        public List<StateInfo> GetStates()
        {
            string _filePath = @"DataFiles\Taxes.txt";

            List<StateInfo> states = new List<StateInfo>();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var state = new StateInfo();

                state.StateAbb = columns[0];
                state.StateName = columns[1];
                state.TaxRate = decimal.Parse(columns[2]);

                states.Add(state);
            }

            return states;
        }

        public StateInfo GetState(string StateAbb)
        {
            List<StateInfo> states = GetStates();
            foreach (var a in states)
            {
                if (a.StateAbb == StateAbb) return a;
            }
            return null;
        }

        public int GetOrderNumber(string orderDate)
        {
            var orders = GetAllOrders(orderDate);
            var max = orders.Max(a => a.OrderNumber);
            return max + 1;
        }

        public void WriteLine(Response OrderInfo)
        {
            var orders = GetAllOrders("12122054");
            orders.Add(OrderInfo.Order);
        }

        public Order CheckForOrder(string orderDate, int orderNumber)
        {
            var orders = GetAllOrders(orderDate);
            return orders.FirstOrDefault(a => a.OrderNumber == orderNumber);
        }

        public void DeleteOrder(Response order)
        {
            var orders = GetAllOrders("");
            orders.Remove(order.Order);
        }

        public void ChangeOrder(Response order)
        {
            DeleteOrder(order);
            WriteLine(order);
        }


    }
}
