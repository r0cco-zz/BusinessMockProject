using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Data
{
    public class OrderRepository
    {
        private const string _filePath = @"DataFiles\Bank.txt";

        public List<Order> GetAllAccounts()
        {
            List<Order> accounts = new List<Order>();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var order = new Order();

                order.OrderNumber = int.Parse(columns[0]);
                order.CustomerName = columns[1];
                order.TaxRate = decimal.Parse(columns[2]);
                order.ProductType = columns[3];
                order.Area = decimal.Parse(columns[4]);
                order.CostPerSqFt = decimal.Parse(columns[5]);
                order.LaborCostPerSqFt = decimal.Parse(columns[6]);
                order.MaterialCost = decimal.Parse(columns[6]);
                order.LaborCost = decimal.Parse(columns[6]);
                order.Tax = decimal.Parse(columns[6]);
                order.Total = decimal.Parse(columns[6]);

                accounts.Add(order);
            }

            return orders;
        }
    }
}
