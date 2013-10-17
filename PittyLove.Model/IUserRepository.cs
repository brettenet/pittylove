namespace PittyLove.Model
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByCredentials(string username,string password);
    }
}