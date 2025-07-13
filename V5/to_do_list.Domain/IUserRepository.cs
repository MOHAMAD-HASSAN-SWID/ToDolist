using to_do_list.Domain.Entities;

namespace to_do_list.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindUserAsync(int id, string userName);
        Task<int> AddUserAsync(User user);
    }
}
