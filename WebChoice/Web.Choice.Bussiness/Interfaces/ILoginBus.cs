namespace Web.Choice.Bussiness.Interfaces
{
    public interface ILoginBus
    {
        bool IsValid(string username, string password);
    }
}
