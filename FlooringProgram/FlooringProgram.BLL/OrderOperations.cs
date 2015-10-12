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
        public Response GetOrder(string orderNumber)
        {
            var repo = new OrderRepository();

            var response = new Response();

            var order = repo.GetOrder(orderNumber);

            if (order == null)
            {
                response.Success = false;
                response.Message = "This is not the order you are looking for...";
            }
            else
            {
                response.Success = true;
                response.OrderInfo = order;
            }

            return response;
        }
    }
}
