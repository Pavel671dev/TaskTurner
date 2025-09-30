using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.DataServices;

public class TaskDataService
{
    private readonly string filePath;
    private readonly string folderName = "TaskTurner";
    private readonly string fileName = "tasks.json";

    public TaskDataService()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataPath, folderName);
        string dataFolder = Path.Combine(appFolder, "data");

        // Create directory if the folder doesn't exist
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
        
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
        List<Task> tasks = new List<Task>();
        string fileContent = File.ReadAllText(filePath);
        tasks = JsonConvert.DeserializeObject<List<Task>>(fileContent);
        return tasks;
    }

    public void SaveTasks(List<Task> tasks)
    {
        //Serialize and write the list of tasks to the json file
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
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
        tasks.RemoveAll(t =>  t.Id == taskId);
        
        SaveTasks(tasks);
    }
    public int GenerateNewTaskID()
    {
        var tasks = LoadTasks();
        if (!tasks.Any())
        {
            return 1;
        }
        int maxId = tasks.Max(t => t.Id);
        return maxId + 1;
    }
}