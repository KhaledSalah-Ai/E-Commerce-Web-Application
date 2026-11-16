namespace NovaCart.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string userId, string email, string firstName, string lastName);
    }
}