using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FlooringProgram.Data
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAllOrders(int orderDate)
        {
            string _filePath = String.Format(@"DataFiles\Orders_{0}.txt", orderDate); // gonna change for more files

            GetProducts();

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

        public List<ProductTypes> GetProducts()
        {

            string _filePath = @"DataFiles\ProductTypes.txt";


            List<ProductTypes> products = new List<ProductTypes>();

            var product = new ProductTypes();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                product.ProductType = columns[0];
                product.MaterialCost = decimal.Parse(columns[1]);
                product.LaborCost = decimal.Parse(columns[2]);

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
            string _filePath = @"DataFiles\States.txt";

            List<StateInfo> states = new List<StateInfo>();

            var state = new StateInfo();

            var reader = File.ReadAllLines(_filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                state.StateAbb = columns[0];
                state.StateName = columns[1];
                state.TaxRate = decimal.Parse(columns[2]);

            }

            return states;
        }

        public StateInfo GetState(string StateAbb)
        {
            List<StateInfo> states = GetStates();
            return states.FirstOrDefault(a => a.StateAbb == StateAbb);
        }

        public int GetOrderNumber(int orderDate)
        {
            bool alreadyOrder = File.Exists(String.Format(@"DataFiles\Orders_{0}", orderDate));
            if (alreadyOrder)
            {
                var orders = GetAllOrders(orderDate);
                return orders.Count + 1;
            }
            return 1;
        }

    }
}  








   
