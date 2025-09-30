namespace TaskTurner.Models;

public class Subtask
{
    public string Description { get; set; }
    public string IsCompleted { get; set; }

    public Subtask(string description)
    {
        Description = description;
        
    }
}