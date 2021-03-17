namespace Billing.Api.Data.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public User Get(int id)
        {
            return _applicationDbContext.Users.Find(id);
        }
    }
}
