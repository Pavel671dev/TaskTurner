namespace TaskTurner.Models;

public class Subtask
{
    public Subtask(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
    public string IsCompleted { get; set; }
}