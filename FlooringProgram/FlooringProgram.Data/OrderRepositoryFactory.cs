using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;

namespace FlooringProgram.Data
{
    public  class OrderRepositoryFactory
    {
        private string _sAttr = ConfigurationManager.AppSettings["Mode"];

        public static IOrderRepository CreateOrderRepository(string _sAttr)
        {
            switch (_sAttr)
            {
                case "Dev":
                    return new OrderRepository();

                case "Prod":
                    return new MockOrderRepository();

                default:
                    throw new NotSupportedException(String.Format("{0} not supported!", _sAttr));
            }
        }
    }
}
