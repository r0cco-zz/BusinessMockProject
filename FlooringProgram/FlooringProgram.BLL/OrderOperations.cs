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
            newOrder.CustomerName = customerName; //clean these calculations up (possibly new method?)
            newOrder.Area = area;
            newOrder.OrderNumber = repo.GetOrderNumber(orderDate);
            newOrder.OrderDate = orderDate;
            newOrder.ProductType = repo.GetProduct(productType.Substring(0,1).ToUpper() + productType.Substring(1).ToLower());
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
                return response;
            }
           
        }

        public Response RemoveOrder(int orderDate, int orderNumber)
        {
            var repo = new OrderRepository();
            var currentOrder = repo.CheckForOrder(orderDate, orderNumber);

            var response = new Response();
            if (currentOrder != null)
            {
                response.Success = true;
                response.Message = "We found an order matching that data.";
                response.Order = currentOrder;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "We did not find an order matching that data.";
                return response;
            }


        }

        public void PassAddToData(Response OrderInfo)
        {
            var BLLOrder = new Response();
            BLLOrder = OrderInfo;

            var or = new OrderRepository();

            //this method actually writes the order data on the file
            or.WriteLine(BLLOrder);
        }

        public void PassRemoveFromData(Response response)
        {
            var BLLRemoveOrder = new Response();
            BLLRemoveOrder = response;

            var repo = new OrderRepository();
            repo.DeleteOrder(BLLRemoveOrder);
        }

        public Response EditOrder(int orderDate, int orderNumber)
        {
            var repo = new OrderRepository();
            var currentOrder = repo.CheckForOrder(orderDate, orderNumber);

            var response = new Response();
            if (currentOrder != null)
            {
                response.Success = true;
                response.Message = "We found an order matching that data.";
                response.Order = currentOrder;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "We did not find an order matching that data.";
                return response;
            }
        }

        public Response EditedOrder(Response OrderInfo)
        {
            var repo = new OrderRepository(); //change to interface type later
            Order newOrder = new Order();
            newOrder.CustomerName = OrderInfo.Order.CustomerName; //clean these calculations up (possibly new method?)
            newOrder.Area = OrderInfo.Order.Area;
            newOrder.OrderNumber = OrderInfo.Order.OrderNumber;
            newOrder.OrderDate = OrderInfo.Order.OrderDate;
            newOrder.ProductType = OrderInfo.Order.ProductType;
            newOrder.State = OrderInfo.Order.State;
            newOrder.TaxRate = OrderInfo.Order.TaxRate;
            decimal MatCost = OrderInfo.Order.Area * newOrder.ProductType.MaterialCost;
            newOrder.MaterialCost = MatCost;
            decimal LabCost = OrderInfo.Order.Area * newOrder.ProductType.LaborCost;
            newOrder.LaborCost = LabCost;
            decimal tax = (MatCost + LabCost) * (OrderInfo.Order.TaxRate / 100);
            newOrder.Tax = tax;
            newOrder.Total = MatCost + LabCost + tax;

            var response = new Response();

            if (true)
            {
                response.Success = true;
                response.Order = newOrder;
                return response;
            }
        }

        public void PassEditBLL(Response response)
        {
            var repo = new OrderRepository();
            repo.ChangeOrder(response);
        }


    }
}
