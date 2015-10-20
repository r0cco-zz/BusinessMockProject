using System;
using FlooringProgram.Models;

namespace FlooringProgram.Data
{
    public class OrderRepositoryFactory
    {
       

        public static IOrderRepository CreateOrderRepository(string sAttr)
        {
            switch (sAttr)
            {
                case "Prod":
                    return new OrderRepository();

                case "Dev":
                    return new MockOrderRepository();

                default:
                    throw new NotSupportedException(String.Format("{0} not supported!", sAttr));
            }
        }
    }
}
