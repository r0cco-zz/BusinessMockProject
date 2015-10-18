using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Data;
using NUnit.Framework;

namespace FlooringProgram.Tests
{
    [TestFixture]
    public class BLLTests
    {

        [Test]
        public void AddNewTest()
        {
            var repo = new MockOrderRepository();
            //var results = repo.GetAllOrders();


        }
    }
}
