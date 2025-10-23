using System.Collections.ObjectModel;

namespace TaskTurner.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsCompleted { get; set; }
    public TimeSpan Timer { get; set; }

    public TaskState TaskState { get; set; }
    public TaskCategory TaskCategory { get; set; }
    public TaskImportance TaskImportance { get; set; }

    public ObservableCollection<Subtask> TaskCheckList { get; set; }
}