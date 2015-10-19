using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Data;
using NUnit.Framework;
using FlooringProgram.Models;

namespace FlooringProgram.Tests
{
    [TestFixture]
    public class DataTests
    {
        public MockOrderRepository target;

        [SetUp]
        public void StartUp()
        {
            target = new MockOrderRepository();
        }

        [Test]
        public void returnAdd()
        {
            int expected = 5;
            Response orderInfo = new Response();
            orderInfo.Success = true;
            orderInfo.Message = "Hi";

            orderInfo.Order = new Order
            {
                OrderNumber = 5,
                CustomerName = "Shem",
                State = "PA",
                TaxRate = (decimal) 6.25,
                ProductType = new ProductTypes
                {
                    ProductType = "Wood",
                    MaterialCost = (decimal) 5.15,
                    LaborCost = (decimal) 4.75
                },
                Area = (decimal) 100,
                MaterialCost = (decimal) 515.00,
                LaborCost = (decimal) 475.00,
                Tax = (decimal) 61.875000,
                Total = (decimal) 1051.875000
            };

            target.WriteLine(orderInfo);
            int result = target.GetAllOrders("12122054").Count;
            Assert.AreEqual(result, expected);
        }
        [Test]
        public void returnRemove()
        {
            int expected = 3;
            Response orderInfo = new Response();
            orderInfo.Success = true;
            orderInfo.Message = "Hi";
            orderInfo.Order = new Order
            {
                OrderNumber = 4,
                CustomerName = "Sam",
                State = "OH",
                TaxRate = (decimal)6.25,
                ProductType = new ProductTypes
                {
                    ProductType = "Wood",
                    MaterialCost = (decimal)5.15,
                    LaborCost = (decimal)4.75
                },
                Area = (decimal)100,
                MaterialCost = (decimal)515.00,
                LaborCost = (decimal)475.00,
                Tax = (decimal)61.875000,
                Total = (decimal)1051.875000
            };
            target.DeleteOrder(orderInfo);
            int result = target.GetAllOrders("12122054").Count;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void returnEdit()
        {
            string expected = "Sam";
            Response orderInfo = new Response();
            orderInfo.Success = true;
            orderInfo.Message = "Hi";
            orderInfo.Order = new Order
            {
                OrderNumber = 4,
                CustomerName = "Willie",
                State = "OH",
                TaxRate = (decimal)6.25,
                ProductType = new ProductTypes
                {
                    ProductType = "Wood",
                    MaterialCost = (decimal)5.15,
                    LaborCost = (decimal)4.75
                },
                Area = (decimal)100,
                MaterialCost = (decimal)515.00,
                LaborCost = (decimal)475.00,
                Tax = (decimal)61.875000,
                Total = (decimal)1051.875000
            };
            target.ChangeOrder(orderInfo);
            Assert.AreNotEqual(orderInfo.Order.CustomerName, expected);
        }

        

    }

}