namespace HPTA.Services.Contracts
{
    public interface IJwtTokenService
    {
        string GenerateToken(string email);
    }
}
