using ProductApi.Domain.Entities;
using E_CommerceSharedLibrary.Interface; // If you have generic interface

namespace ProductApi.Application.Interface
{
    public interface IProductInterface : IGenericInterface<Product> { }
}
