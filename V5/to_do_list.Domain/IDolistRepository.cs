using System.Collections.Generic;
using System.Threading.Tasks;
using to_do_list.Domain.Entities;

namespace to_do_list.Domain.Interfaces
{
    public interface IDolistRepository
    {
        Task<List<Dolist>> Show_All_DolistAsync(int userId, bool? sortBy, int pageNumber, int pageSize);

        Task<List<Dolist>> Show_Dolist_Filter_By_CategoryAsync(int userId, string category);

        Task<List<Dolist>> Show_Dolist_Filter_By_PriorityAsync(int userId, int priority);

        Task<Dolist> Show_Dolist_Filter_By_IDAsync(int listId);

        Task<int> Add_DolistAsync(Dolist dolist);

        Task<bool> Update_DolistAsync(Dolist dolist);

        Task<bool> Delete_DolistAsync(int listId);

        //Task<bool> Change_State_DolistAsync(int listId, bool completed);
    }
}
