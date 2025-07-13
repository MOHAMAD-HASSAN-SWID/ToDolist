using to_do_list.Domain.Entities;
using to_do_list.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace to_do_list.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserAsync(int id, string userName)
        {
            return await _userRepository.FindUserAsync(id, userName);
        }

        public async Task<int> AddUserAsync(string userName, int permission, DateTime dateRegistered)
        {
            User user = new User(0, userName, permission, dateRegistered);
            return await _userRepository.AddUserAsync(user);
        }
    }
}
