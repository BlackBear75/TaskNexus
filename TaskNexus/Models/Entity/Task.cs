
using TaskNexus.Models.Enum;


namespace TaskNexus.Models.Entity
{
    public class Task_Entity
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enum.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime Deadline { get; set; }
        public string AssignedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

      


    }
}
