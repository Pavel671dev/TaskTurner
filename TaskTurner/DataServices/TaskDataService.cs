using System.IO;
using Newtonsoft.Json;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.DataServices;

public class TaskDataService
{
    private readonly string fileName = "tasks.json";
    private readonly string filePath;
    private readonly string folderName = "TaskTurner";

    public TaskDataService(string appDataPath)
    {
        var appFolder = Path.Combine(appDataPath, folderName);
        var dataFolder = Path.Combine(appFolder, "data");

        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(appFolder, fileName);
        InitializeFile();
    }

    public List<Task> LoadTasks()
    {
        var tasks = new List<Task>();
        var fileContent = File.ReadAllText(filePath);
        tasks = JsonConvert.DeserializeObject<List<Task>>(fileContent);
        return tasks;
    }

    public void AddTask(Task newTask)
    {
        newTask.Id = GenerateNewTaskId();
        var tasks = LoadTasks();
        tasks.Add(newTask);
        SaveTasks(tasks);
    }

    public void UpdateTask(Task updateTask)
    {
        var tasks = LoadTasks();
        var index = tasks.FindIndex(t => t.Id == updateTask.Id);

        if (index != -1)
        {
            tasks[index] = updateTask;
            SaveTasks(tasks);
        }
    }

    public void DeleteTasks(int taskId)
    {
        var tasks = LoadTasks();
        tasks.RemoveAll(t => t.Id == taskId);

        SaveTasks(tasks);
    }

    public int GenerateNewTaskId()
    {
        var tasks = LoadTasks();
        if (!tasks.Any())
        {
            return 1;
        }
        var maxId = tasks.Max(t => t.Id);
        return maxId + 1;
    }

    private void InitializeFile()
    {
        // Create json and adds the structure
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(new List<Task>()));
        }
    }

    private void SaveTasks(List<Task> tasks)
    {
        var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}