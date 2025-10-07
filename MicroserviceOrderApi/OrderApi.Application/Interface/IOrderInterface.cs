using E_CommerceSharedLibrary.Interface;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Interface
{
    public interface IOrderInterface : IGenericInterface<Order>
    {
       
        Task<IEnumerable<Order>> GetOrdersByAsync(Func<Order, bool> predicate);
    }
}
