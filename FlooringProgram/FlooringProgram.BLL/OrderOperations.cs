using FlooringProgram.Models;

namespace FlooringProgram.BLL
{
    public class OrderOperations
    {
        private IOrderRepository _repo;

        public OrderOperations(IOrderRepository repo)
        {
            _repo = repo;
        }

        public Response GetAllOrdersFromDate(string orderDate)
        {
            //var repo = new OrderRepository();

            var response = new Response();

            var orders = _repo.GetAllOrders(orderDate);

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

        public Response AddOrder(string orderDate, string customerName, string state, string productType, decimal area)
        {
            //var repo = new OrderRepository(); //change to interface type later
            Order newOrder = new Order();
            newOrder.CustomerName = customerName; //clean these calculations up (possibly new method?)
            newOrder.Area = area;
            newOrder.OrderNumber = _repo.GetOrderNumber(orderDate);
            newOrder.OrderDate = orderDate;
            newOrder.ProductType =
                _repo.GetProduct(productType.Substring(0, 1).ToUpper() + productType.Substring(1).ToLower());
            var currentState = _repo.GetState(state);
            newOrder.State = currentState.StateAbb;
            newOrder.TaxRate = currentState.TaxRate;
            decimal matCost = area*newOrder.ProductType.MaterialCost;
            newOrder.MaterialCost = matCost;
            decimal labCost = area*newOrder.ProductType.LaborCost;
            newOrder.LaborCost = labCost;
            decimal tax = (matCost + labCost)*(currentState.TaxRate/100);
            newOrder.Tax = tax;
            newOrder.Total = matCost + labCost + tax;

            var response = new Response();

            if (true)
            {
                response.Success = true;
                response.Order = newOrder;
                return response;
            }

        }

        public Response RemoveOrder(string orderDate, int orderNumber)
        {
            //var repo = new OrderRepository();
            var currentOrder = _repo.CheckForOrder(orderDate, orderNumber);

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

        public void PassAddToData(Response orderInfo)
        {
            var bllOrder = orderInfo;

            //var or = new OrderRepository();

            //this method actually writes the order data on the file
            _repo.WriteLine(bllOrder);
        }

        public void PassRemoveFromData(Response response)
        {
            var bllRemoveOrder = response;

            //var repo = new OrderRepository();
            _repo.DeleteOrder(bllRemoveOrder);
        }

        public Response EditOrder(string orderDate, int orderNumber)
        {
            //var repo = new OrderRepository();
            var currentOrder = _repo.CheckForOrder(orderDate, orderNumber);

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

        public Response EditedOrder(Response orderInfo)
        {
            //var repo = new OrderRepository(); //change to interface type later
            Order newOrder = new Order
            {
                CustomerName = orderInfo.Order.CustomerName,
                Area = orderInfo.Order.Area,
                OrderNumber = orderInfo.Order.OrderNumber,
                OrderDate = orderInfo.Order.OrderDate,
                ProductType = orderInfo.Order.ProductType,
                State = orderInfo.Order.State,
                TaxRate = orderInfo.Order.TaxRate
            };
            //clean these calculations up (possibly new method?)
            decimal matCost = orderInfo.Order.Area*newOrder.ProductType.MaterialCost;
            newOrder.MaterialCost = matCost;
            decimal labCost = orderInfo.Order.Area*newOrder.ProductType.LaborCost;
            newOrder.LaborCost = labCost;
            decimal tax = (matCost + labCost)*(orderInfo.Order.TaxRate/100);
            newOrder.Tax = tax;
            newOrder.Total = matCost + labCost + tax;

            var response = new Response();

            if (true)
            {
                response.Success = true;
                response.Order = newOrder;
                return response;
            }
        }

        public void PassEditBll(Response response)
        {
            //var repo = new OrderRepository();
            _repo.ChangeOrder(response);
        }

        public void ErrorPassdown(ErrorLogger log)
        {
            //var repo = new OrderRepository();
            _repo.WriteError(log);
        }
    }
}
