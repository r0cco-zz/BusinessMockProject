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

        Dictionary<string, List<Order>> allOrdersOnDate = new Dictionary<string, List<Order>>();

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

            var results =
                _orders.OrderBy(o => o.OrderDate)
                    .GroupBy(o => o.OrderDate)
                    .Select(dateOrders => new {Date = dateOrders.Key, Orders = dateOrders});

            foreach (var result in results)
            {
                allOrdersOnDate.Add(result.Date, new List<Order>(result.Orders));
            }

    } 

        public List<Order> GetAllOrders(string orderDate)
        {
            List<Order> DateOrderList = new List<Order>();

            if (allOrdersOnDate.ContainsKey(orderDate) && allOrdersOnDate[orderDate] != null)
            {
                DateOrderList = allOrdersOnDate[orderDate];
                return DateOrderList;
            }

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
            bool alreadyOrder = allOrdersOnDate.ContainsKey(orderDate);
            if (alreadyOrder)
            {
                var orders = GetAllOrders(orderDate);
                var max = orders.Max(a => a.OrderNumber);
                return max + 1;
            }
            return 1;
        }

        public void WriteLine(Response orderInfo)
        {
            var DateOrderList = new List<Order>();

            if (allOrdersOnDate.ContainsKey(orderInfo.Order.OrderDate))
            {
                DateOrderList = allOrdersOnDate[orderInfo.Order.OrderDate];
                DateOrderList.Add(orderInfo.Order);

                allOrdersOnDate.Remove(orderInfo.Order.OrderDate);
                allOrdersOnDate.Add(orderInfo.Order.OrderDate, DateOrderList);
            }
            else
            {
                DateOrderList.Add(orderInfo.Order);

                allOrdersOnDate.Add(orderInfo.Order.OrderDate, DateOrderList);
            }
        }

        public Order CheckForOrder(string orderDate, int orderNumber)
        {
            var DateOrderList = allOrdersOnDate[orderDate];

            return DateOrderList.FirstOrDefault(a => a.OrderNumber == orderNumber);
        }

        public void DeleteOrder(Response order)
        {
            var DateOrderList = allOrdersOnDate[order.Order.OrderDate];
            DateOrderList.Remove(order.Order);


            if (DateOrderList.Count == 0)
            {
                allOrdersOnDate.Remove(order.Order.OrderDate);
            }
            else
            {
                allOrdersOnDate.Remove(order.Order.OrderDate);
                allOrdersOnDate.Add(order.Order.OrderDate, DateOrderList);
            }
        }

        public void ChangeOrder(Response order)
        {
            DeleteOrder(order);
            //WriteLine(order);
        }

        public void WriteError(ErrorLogger log)
        {
            using (var writer = File.AppendText(String.Format(@"DataFiles\log.txt")))
            {
                writer.WriteLine("{0:s} : {1}", log.TimeOfError, log.Message);
            }
        }

        public bool DoesDateExist(string orderDate)
        {
            if (allOrdersOnDate.ContainsKey(orderDate))
            {
                return true;
            }
            return false;
        }
    }
}
