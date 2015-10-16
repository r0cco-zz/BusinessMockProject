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
        public List<Order> _orders = new List<Order>();

        public MockOrderRepository()
        {
            string _filePath = @"DataFiles\Orders_12122054.txt";

            GetProducts();

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

                _orders.Add(order);
            }

           
        } 

        public List<Order> GetAllOrders(string orderDate)
        {
            return _orders;
        }

        public List<ProductTypes> GetProducts()
        {

            string _filePath = @"DataFiles\Products.txt";


            List<ProductTypes> products = new List<ProductTypes>();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var product = new ProductTypes
                {
                    ProductType = columns[0],
                    MaterialCost = decimal.Parse(columns[1]),
                    LaborCost = decimal.Parse(columns[2])
                };


                products.Add(product);
            }

            return products;
        }

        public ProductTypes GetProduct(string productType)
        {
            List<ProductTypes> products = GetProducts();
            return products.FirstOrDefault(a => a.ProductType == productType);
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

        public StateInfo GetState(string stateAbb)
        {
            List<StateInfo> states = GetStates();
            foreach (var a in states)
            {
                if (a.StateAbb == stateAbb) return a;
            }
            return null;
        }

        public int GetOrderNumber(string orderDate)
        {
            var max = _orders.Max(a => a.OrderNumber);
            return max + 1;
        }

        public void WriteLine(Response orderInfo)
        {
            _orders.Add(orderInfo.Order);
        }

        public Order CheckForOrder(string orderDate, int orderNumber)
        {
            return _orders.FirstOrDefault(a => a.OrderNumber == orderNumber);
        }

        public void DeleteOrder(Response order)
        {
            _orders.Remove(order.Order);
        }

        public void ChangeOrder(Response order)
        {
            DeleteOrder(order);
            WriteLine(order);
        }

        public void WriteError(ErrorLogger log)
        {
            using (var writer = File.AppendText(String.Format(@"DataFiles\log.txt")))
            {
                writer.WriteLine("{0:s} : {1}", log.TimeOfError, log.Message);
            }
        }


    }
}
