namespace to_do_list.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Permission { get; set; }
        public DateTime DateRegistered { get; set; }

        public ICollection<Dolist> Dolists { get; set; }
        public User() { }

        public User(int id, string userName, int permission, DateTime dateRegistered)
        {
            Id = id;
            UserName = userName;
            Permission = permission;
            DateRegistered = dateRegistered;
        }
    }


}
