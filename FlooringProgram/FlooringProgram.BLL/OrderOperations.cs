using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Data;
using FlooringProgram.Models;

namespace FlooringProgram.BLL
{
    public class OrderOperations
    {
        public Response GetAllOrdersFromDate(int orderDate)
        {
            var repo = new OrderRepository();

            var response = new Response();

            var orders = repo.GetAllOrders(orderDate);

            if (orders == null)
            {
                response.Success = false;
                response.Message = "This is not the order you are looking for...";
            }
            else
            {
                response.Success = true;
                response.OrderList = orders;
            }

            return response;
        }

        public Response AddOrder(int orderDate, string customerName, string state, string productType, decimal area)
        {
            var repo = new OrderRepository(); //change to interface type later
            Order newOrder = new Order();
            newOrder.CustomerName = customerName;
            newOrder.Area = area;
            newOrder.OrderNumber = repo.GetOrderNumber(orderDate);
            newOrder.ProductType = repo.GetProduct(productType);
            var currentState = repo.GetState(state);
            newOrder.State = currentState.StateAbb;
            newOrder.TaxRate = currentState.TaxRate;
            decimal MatCost = area * newOrder.ProductType.MaterialCost;
            newOrder.MaterialCost = MatCost;
            decimal LabCost = area * newOrder.ProductType.LaborCost;
            newOrder.LaborCost = LabCost;
            decimal tax = (MatCost + LabCost) * (currentState.TaxRate/100);
            newOrder.Tax = tax;
            newOrder.Total = MatCost + LabCost + tax;

            var response = new Response();

            if (true)
            {
                response.Success = true;
                response.Order = newOrder;
            }
            else
            {
                response.Success = false;
                response.Message = "There was a problem with creating your order";
            }
            return response;
        }
    }
}
