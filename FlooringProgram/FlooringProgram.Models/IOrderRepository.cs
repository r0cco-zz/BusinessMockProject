using System.Collections.Generic;

namespace FlooringProgram.Models
{

    public interface IOrderRepository
    {
        List<Order> GetAllOrders(string orderDate);

        List<ProductTypes> GetProducts();

        ProductTypes GetProduct(string productType);

        List<StateInfo> GetStates();

        StateInfo GetState(string stateAbb);

        int GetOrderNumber(string orderDate);

        void WriteLine(Response orderInfo);

        Order CheckForOrder(string orderDate, int orderNumber);

        void DeleteOrder(Response order);

        void ChangeOrder(Response order);

        void WriteError(ErrorLogger log);

        bool DoesDateExist(string orderDate);
    }
}
