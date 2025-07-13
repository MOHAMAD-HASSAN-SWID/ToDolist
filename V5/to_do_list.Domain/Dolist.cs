namespace to_do_list.Domain.Entities
{
    public class Dolist
    {
        public int ListID { get; set; }
        public int UserID { get; set; }
        public string ListTitle { get; set; }
        public string ListBody { get; set; }
        public bool Completed { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }

        public User user { get; set; }

        public Dolist() { }

        public Dolist(int listID, int userID, string listTitle, string listBody, bool completed, string category, int priority)
        {
            ListID = listID;
            UserID = userID;
            ListTitle = listTitle;
            ListBody = listBody;
            Completed = completed;
            Category = category;
            Priority = priority;
        }
    }
}
