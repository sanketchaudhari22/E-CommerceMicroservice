namespace ProductApi.Application.DTOs
{
    public record ProductDTO(
        int Id,
        string Name,
        int Quantity,
        decimal Price
    );
}
