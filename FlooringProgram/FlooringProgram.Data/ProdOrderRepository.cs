using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;

namespace FlooringProgram.Data
{
    public class ProdOrderRepository : IOrderRepository
    {
        public List<Order> GetAllOrders(int orderDate)
        {
            string _filePath = String.Format(@"DataFiles\Orders_{0}.txt", orderDate);  // gonna change for more files

            List<Order> orders = new List<Order>();

            
            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var order = new Order();
                order.ProductType = new ProductTypes();

                order.OrderNumber = int.Parse(columns[0]);
                order.CustomerName = columns[1];
                order.State = columns[2];
                order.TaxRate = decimal.Parse(columns[3]);
                order.ProductType.ProductType = (columns[4]);
                order.Area = decimal.Parse(columns[5]);
                order.ProductType.MaterialCost = decimal.Parse(columns[6]);
                order.ProductType.LaborCost = decimal.Parse(columns[7]);
                order.MaterialCost = decimal.Parse(columns[8]);
                order.LaborCost = decimal.Parse(columns[9]);
                order.Tax = decimal.Parse(columns[10]);
                order.Total = decimal.Parse(columns[11]);

                orders.Add(order);
            }

            return orders;
        }
    }
}
