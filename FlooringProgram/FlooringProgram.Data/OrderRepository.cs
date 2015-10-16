using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace FlooringProgram.Data
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAllOrders(string orderDate)
        {
            string _filePath = String.Format(@"DataFiles\Orders_{0}.txt", orderDate); // gonna change for more files

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
            
                for (int j = 1; j < columns.Count()-11; j++)
                {
                    order.CustomerName += columns[j] +",";
                }
                order.CustomerName += columns[columns.Count() - 11];

                order.State = columns[columns.Count()-10];
                order.TaxRate = decimal.Parse(columns[columns.Count()-9]);
                order.ProductType.ProductType = (columns[columns.Count()-8]);
                order.Area = decimal.Parse(columns[columns.Count()-7]);
                order.ProductType.MaterialCost = decimal.Parse(columns[columns.Count()-6]);
                order.ProductType.LaborCost = decimal.Parse(columns[columns.Count()-5]);
                order.MaterialCost = decimal.Parse(columns[columns.Count()-4]);
                order.LaborCost = decimal.Parse(columns[columns.Count()-3]);
                order.Tax = decimal.Parse(columns[columns.Count()-2]);
                order.Total = decimal.Parse(columns[columns.Count()-1]);

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
            bool alreadyOrder = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDate));
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
           bool alreadyOrder = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderInfo.Order.OrderDate));

            if (alreadyOrder)
            {
                using (
                    var writer = File.AppendText(String.Format(@"DataFiles\Orders_{0}.txt", orderInfo.Order.OrderDate)))
                {
                    writer.WriteLine("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", orderInfo.Order.OrderNumber,
                        orderInfo.Order.CustomerName, orderInfo.Order.State,
                        orderInfo.Order.TaxRate, orderInfo.Order.ProductType.ProductType, orderInfo.Order.Area,
                        orderInfo.Order.ProductType.MaterialCost, orderInfo.Order.ProductType.LaborCost,
                        orderInfo.Order.MaterialCost, orderInfo.Order.LaborCost, orderInfo.Order.Tax,
                        orderInfo.Order.Total);
                }
            }
            else
            {
                using (
                    var writer = File.CreateText(String.Format(@"DataFiles\Orders_{0}.txt", orderInfo.Order.OrderDate)))
                {

                    writer.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                    writer.WriteLine("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", orderInfo.Order.OrderNumber,
                        orderInfo.Order.CustomerName, orderInfo.Order.State,
                        orderInfo.Order.TaxRate, orderInfo.Order.ProductType.ProductType, orderInfo.Order.Area,
                        orderInfo.Order.ProductType.MaterialCost, orderInfo.Order.ProductType.LaborCost,
                        orderInfo.Order.MaterialCost, orderInfo.Order.LaborCost, orderInfo.Order.Tax,
                        orderInfo.Order.Total);
                }
            }
        }

        public Order CheckForOrder(string orderDate, int orderNumber)
        {
            string filePath = (String.Format(@"DataFiles\Orders_{0}.txt", orderDate));
            bool alreadyOrder = File.Exists(filePath);
            if (alreadyOrder)
            {
                var orders = GetAllOrders(orderDate);
                return orders.FirstOrDefault(a => a.OrderNumber == orderNumber);
            }
            return null;
        }

        public void DeleteOrder(Response order)
        {
            string filePath = (String.Format(@"DataFiles\Orders_{0}.txt", order.Order.OrderDate));
            var orders = GetAllOrders(order.Order.OrderDate);
            if (orders.Count == 1)
            {
                File.Delete(filePath);
            }
            else
            {
                orders = orders.Where(a => a.OrderNumber != order.Order.OrderNumber).ToList();
                //orders.Remove(orderToRemove);
                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                    foreach (var newo in orders)
                    {
                        writer.WriteLine("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", newo.OrderNumber,
                       newo.CustomerName, newo.State, newo.TaxRate, newo.ProductType.ProductType, newo.Area,
                       newo.ProductType.MaterialCost, newo.ProductType.LaborCost, newo.MaterialCost, newo.LaborCost, newo.Tax,
                       newo.Total);
                    }
                }
            }
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








   
