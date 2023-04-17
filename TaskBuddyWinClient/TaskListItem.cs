using TaskBuddyClassLibrary.Models;

namespace TaskBuddyWinClient
{
    public class TaskListItem
    {
        public TaskListItem(TaskBuddyTask taskItem)
        {
            this.TaskItem = taskItem;
            Id = taskItem.Id;
            Title = taskItem.Title;
            Description = taskItem.Description;
            IsComplete = taskItem.IsComplete;
        }

        public TaskBuddyTask TaskItem { get; set; }
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

}