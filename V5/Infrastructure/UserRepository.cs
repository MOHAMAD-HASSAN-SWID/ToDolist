using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using to_do_list.Domain.Entities;
using to_do_list.Domain.Interfaces;
using to_do_list.Infrastructure.Data;

namespace to_do_list.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindUserAsync(int id, string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.UserName == userName);
        }

        public async Task<int> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
    }
}
