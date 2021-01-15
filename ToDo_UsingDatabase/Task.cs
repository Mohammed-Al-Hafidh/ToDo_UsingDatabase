using System.ComponentModel.DataAnnotations;

namespace ToDo_UsingDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Task
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string TaskName { get; set; }
        public string DueDate { get; set; }
        public int Difficulty { get; set; }
        public string Status { get; set; }
        public Task()
        {
            
        }
        public Task(string taskName, string dueDate, int difficulty, string status)
        {            
            this.TaskName = taskName;
            this.DueDate = dueDate;
            this.Difficulty = difficulty;
            this.Status = status;
        }
        public Task(int id,string taskName, string dueDate, int difficulty, string status)
        {
            this.Id = id;
            this.TaskName = taskName;
            this.DueDate = dueDate;
            this.Difficulty = difficulty;
            this.Status = status;
        }
        public override string ToString()
        {
            return string.Format("{0} {1} by {2} / {3}, {4}",this.Id, this.TaskName, this.DueDate, this.Difficulty, this.Status);
        }
        public string ToDataString()
        {
            return string.Format("{0};{1};{2};{3}", this.TaskName, this.DueDate, this.Difficulty, this.Status);
        }
    }
}
