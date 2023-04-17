namespace TaskBuddyClassLibrary.Models
{
    public class TaskBuddyTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }


}
