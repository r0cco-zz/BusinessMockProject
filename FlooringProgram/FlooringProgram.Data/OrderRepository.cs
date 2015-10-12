using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using System.IO;

namespace FlooringProgram.Data
{
    public class OrderRepository
    {
        private const string _filePath = @"DataFiles\Orders_10112015.txt";  // gonna change for more files

        public List<Order> GetAllOrders(int orderDate)
        {
            List<Order> orders = new List<Order>();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var order = new Order();

                order.OrderNumber = int.Parse(columns[0]);
                order.CustomerName = columns[1];
                order.State = columns[2];
                order.TaxRate = decimal.Parse(columns[3]);
                order.ProductType = (columns[4]);
                order.Area = decimal.Parse(columns[5]);
                order.MaterialCostPerSqFt = decimal.Parse(columns[6]);
                order.LaborCostPerSqFt = decimal.Parse(columns[7]);
                order.MaterialCost = decimal.Parse(columns[8]);
                order.LaborCost = decimal.Parse(columns[9]);
                order.Tax = decimal.Parse(columns[10]);
                order.Total = decimal.Parse(columns[11]);

                orders.Add(order);
            }

            return orders;
        }

        //public Order GetOrder(string orderNumber)
        //{
        //    List<Order> accounts = GetAllOrders();

        //    return accounts.FirstOrDefault(a => a. == orderNumber);
        //}


    }
}
