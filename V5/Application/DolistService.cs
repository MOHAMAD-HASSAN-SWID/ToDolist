using to_do_list.Domain.Interfaces;
using to_do_list.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public class DolistService
    {
        private readonly IDolistRepository _dolistRepository;

        public DolistService(IDolistRepository dolistRepository)
        {
            _dolistRepository = dolistRepository;
        }

        public async Task<List<Dolist>> Show_All_DolistAsync(int userId, bool? sortBy, int pageNumber, int pageSize)
        {
            return await _dolistRepository.Show_All_DolistAsync(userId, sortBy, pageNumber, pageSize);
        }

        public async Task<List<Dolist>> Show_Dolist_Filter_By_CategoryAsync(int userId, string category)
        {
            return await _dolistRepository.Show_Dolist_Filter_By_CategoryAsync(userId, category);
        }

        public async Task<List<Dolist>> Show_Dolist_Filter_By_PriorityAsync(int userId, int priority)
        {
            return await _dolistRepository.Show_Dolist_Filter_By_PriorityAsync(userId, priority);
        }

        public async Task<Dolist> Show_Dolist_Filter_By_IDAsync(int listId)
        {
            return await _dolistRepository.Show_Dolist_Filter_By_IDAsync(listId);
        }

        public async Task<int> Add_DolistAsync(int userId, string listTitle, string listBody, bool completed, string category, int priority)
        {
            var dolist = new Dolist(0, userId, listTitle, listBody, completed, category, priority);
            return await _dolistRepository.Add_DolistAsync(dolist);
        }

        public async Task<bool> Update_DolistAsync(int listId, int userId, string listTitle, string listBody, bool completed, string category, int priority)
        {
            var dolist = new Dolist(listId, userId, listTitle, listBody, completed, category, priority);
            return await _dolistRepository.Update_DolistAsync(dolist);
        }

        public async Task<bool> Delete_DolistAsync(int listId)
        {
            return await _dolistRepository.Delete_DolistAsync(listId);
        }

        public async Task<bool> Change_State_DolistAsync(int listId, bool completed)
        {
            var dolist = await Show_Dolist_Filter_By_IDAsync(listId);

            if (dolist == null)
                return false;

            dolist.Completed = completed;

            return await _dolistRepository.Update_DolistAsync(dolist);
        }
    }
}
