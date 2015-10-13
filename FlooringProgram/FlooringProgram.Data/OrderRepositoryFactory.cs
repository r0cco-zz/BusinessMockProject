using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;

namespace FlooringProgram.Data
{
    public static class OrderRepositoryFactory
    {
        //private string sAttr;

        public static IOrderRepository CreateOrderRepository(var x)
        {
            switch (x)
            {
                case "D":
                    return new OrderRepository();

                case "P":
                    return new ProdOrderRepository();

                default:
                    throw new NotSupportedException(String.Format("{0} not supported!", Type));
            }
        }
    }
}
