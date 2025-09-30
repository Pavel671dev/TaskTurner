using System.IO;
using Newtonsoft.Json;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.DataServices;

public class TaskDataService
{
    private readonly string fileName = "tasks.json";
    private readonly string filePath;
    private readonly string folderName = "TaskTurner";

    public TaskDataService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, folderName);
        var dataFolder = Path.Combine(appFolder, "data");

        // Create directory if the folder doesn't exist
        if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

        filePath = Path.Combine(appFolder, fileName);
        InitializeFile();
    }

    private void InitializeFile()
    {
        // Create json and adds the structure
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, JsonConvert.SerializeObject(new List<Task>()));
    }

    public List<Task> LoadTasks()
    {
        //Read tasks from JSON
        var tasks = new List<Task>();
        var fileContent = File.ReadAllText(filePath);
        tasks = JsonConvert.DeserializeObject<List<Task>>(fileContent);
        return tasks;
    }

    public void SaveTasks(List<Task> tasks)
    {
        //Serialize and write the list of tasks to the json file
        var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void AddTask(Task newTask)
    {
        newTask.Id = GenerateNewTaskID();

        //Loading Tasks
        var task = LoadTasks();
        //Adding task and saving
        task.Add(newTask);
        SaveTasks(task);
    }

    public void UpdateTask(Task updateTask)
    {
        var tasks = LoadTasks();
        //Checking id matches the updateTask id 
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

    public int GenerateNewTaskID()
    {
        var tasks = LoadTasks();
        if (!tasks.Any()) return 1;
        var maxId = tasks.Max(t => t.Id);
        return maxId + 1;
    }
}