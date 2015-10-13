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

            var response = new Response();



            return response;
        }
    }
}
