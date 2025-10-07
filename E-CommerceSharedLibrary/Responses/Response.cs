namespace E_CommerceSharedLibrary.Responses
{
    public record Response(bool Success = false, string Message = null!, string? Data = null);
}
