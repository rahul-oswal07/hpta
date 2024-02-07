namespace HPTA.Services.Contracts
{
    public interface IIdentityService
    {
        bool IsAuthenticated();
        string GetEmail();
        string GetId();

        string GetName();
    }
}
