using FlooringProgram.Data;
using NUnit.Framework;
using FlooringProgram.Models;

namespace FlooringProgram.Tests
{
    [TestFixture]
    public class DataTests
    {
        public MockOrderRepository Target;

        [SetUp]
        public void StartUp()
        {
            Target = new MockOrderRepository();
        }

        [Test]
        public void ReturnAdd()
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
                Area = 100,
                MaterialCost = (decimal) 515.00,
                LaborCost = (decimal) 475.00,
                Tax = (decimal) 61.875000,
                Total = (decimal) 1051.875000
            };

            Target.WriteLine(orderInfo);
            int result = Target.GetAllOrders("12122054").Count;
            Assert.AreEqual(result, expected);
        }
        [Test]
        public void ReturnRemove()
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
                Area = 100,
                MaterialCost = (decimal)515.00,
                LaborCost = (decimal)475.00,
                Tax = (decimal)61.875000,
                Total = (decimal)1051.875000
            };
            Target.DeleteOrder(orderInfo);
            int result = Target.GetAllOrders("12122054").Count;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ReturnEdit()
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
                Area = 100,
                MaterialCost = (decimal)515.00,
                LaborCost = (decimal)475.00,
                Tax = (decimal)61.875000,
                Total = (decimal)1051.875000
            };
            Target.ChangeOrder(orderInfo);
            Assert.AreNotEqual(orderInfo.Order.CustomerName, expected);
        }
    }

}