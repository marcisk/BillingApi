using Billing.Api.Data;
using Billing.Api.Data.Repository;
using System.Linq;

namespace Billing.Tests.MockClasses
{
    class UserRepositoryMock : IUserRepository
    {
        private User[] _users = new User[]
        {
            new User { Id = 1, UserName = "test" }
        };

        public User Get(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}
