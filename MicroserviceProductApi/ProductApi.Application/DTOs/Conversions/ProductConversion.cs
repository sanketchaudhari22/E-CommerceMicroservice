using ProductApi.Domain.Entities;
using ProductApi.Application.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace ProductApi.Application.Conversions
{
    public static class ProductConversions
    {
        public static Product ToEntity(ProductDTO product) => new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity
        };

        public static ProductDTO ToDTO(Product product) => new ProductDTO(
            product.Id,
            product.Name,
            product.Quantity,
            product.Price
        );

        public static IEnumerable<ProductDTO> ToDTO(IEnumerable<Product> products) =>
            products.Select(p => ToDTO(p)).ToList();
    }
}
