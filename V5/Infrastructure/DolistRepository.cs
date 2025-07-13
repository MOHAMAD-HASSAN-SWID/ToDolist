using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using to_do_list.Domain.Entities;
using to_do_list.Domain.Interfaces;
using to_do_list.Infrastructure.Data;

namespace Infrastructure
{
    public class DolistRepository : IDolistRepository
    {
        private readonly AppDbContext _context;

        public DolistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Dolist>> Show_All_DolistAsync(int userId, bool? sortBy, int pageNumber, int pageSize)
        {
            var query = _context.Dolists
                                .Where(d => d.UserID == userId);

            if (sortBy.HasValue && sortBy.Value)
            {
                query = query.OrderBy(d => d.Category);
            }
            else if (sortBy.HasValue && !sortBy.Value)
            {
                query = query.OrderByDescending(d => d.Priority);
            }

            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .Select(d => new Dolist
                              {
                                  ListID = d.ListID,
                                  UserID = d.UserID,
                                  ListTitle = d.ListTitle,
                                  ListBody = d.ListBody,
                                  Completed = d.Completed,
                                  Category = d.Category,
                                  Priority = d.Priority,
                              })
                              .ToListAsync();
        }

        public async Task<List<Dolist>> Show_Dolist_Filter_By_CategoryAsync(int userId, string category)
        {
            return await _context.Dolists
                                 .Where(d => d.UserID == userId && d.Category == category)
                                 .ToListAsync();
        }

        public async Task<List<Dolist>> Show_Dolist_Filter_By_PriorityAsync(int userId, int priority)
        {
            return await _context.Dolists
                                 .Where(d => d.UserID == userId && d.Priority == priority)
                                 .ToListAsync();
        }

        public async Task<Dolist> Show_Dolist_Filter_By_IDAsync(int listId)
        {
            return await _context.Dolists
                                 .FirstOrDefaultAsync(d => d.ListID == listId);
        }

        public async Task<int> Add_DolistAsync(Dolist dolist)
        {
            _context.Dolists.Add(dolist);
            await _context.SaveChangesAsync();
            return dolist.ListID;
        }

        public async Task<bool> Update_DolistAsync(Dolist dolist)
        {
            var existing = await _context.Dolists.FindAsync(dolist.ListID);
            if (existing == null) return false;

            existing.ListTitle = dolist.ListTitle;
            existing.ListBody = dolist.ListBody;
            existing.Completed = dolist.Completed;
            existing.Category = dolist.Category;
            existing.Priority = dolist.Priority;
            existing.UserID = dolist.UserID;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete_DolistAsync(int listId)
        {
            var dolist = await _context.Dolists.FindAsync(listId);
            if (dolist == null) return false;

            _context.Dolists.Remove(dolist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
