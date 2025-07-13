using to_do_list.Infrastructure.Data;
using to_do_list.Domain.Entities;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                UserName = "demo_owner",
                Permission = 511,
                DateRegistered = DateTime.UtcNow
            });

            context.SaveChanges();
        }


        if (!context.Dolists.Any())
        {
            context.Dolists.Add(new Dolist
            {
                ListTitle = "first test do list title",
                ListBody = "first test do list body",
                Completed = false,
                Category = "public",
                Priority = 1,
                UserID = 1 
            });

            context.SaveChanges();
        }


    }
}
