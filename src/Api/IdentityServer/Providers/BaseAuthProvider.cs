namespace IdentityServer.Providers
{
    public abstract class BaseAuthProvider
    {
        public abstract string GetEmail();

        public abstract string GetSubject();
    }
}
