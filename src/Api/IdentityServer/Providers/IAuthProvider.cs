namespace IdentityServer.Providers
{
    public interface IAuthProvider
    {
        string GetSubject();
        string GetEmail();
    }
}
