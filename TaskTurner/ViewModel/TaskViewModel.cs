using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TaskTurner.DataServices;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.ViewModel;

public class TaskViewModel: INotifyPropertyChanged
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
    
    public ObservableCollection<TaskChecklist> TaskCheckList { get; set; }
    
    public ICommand IAddNewTask => new RelayCommand(AddNewTask);
    
    private readonly TaskDataService taskDataService;
    
    private ObservableCollection<Task> tasks;

    public ObservableCollection<Task> Tasks
    {
        get => tasks;
        set
        {
            tasks = value;
            OnPropertyChanged(nameof(Tasks));
        }
    }

    private void LoadTasks()
    {
        var TaskList = taskDataService.LoadTasks();
        Tasks = new ObservableCollection<Task>(TaskList);
    }

    public void AddNewTask()
    {
        Task newTask = new Task()
        {
            Title = this.Title,
            Description = this.Description,
            Id = taskDataService.GenerateNewTaskID(),
            DueDate = this.DueDate,
            IsCompleted = false,
            StartDate = DateTime.Now,
            TaskCheckList = this.TaskCheckList,
            TaskImportance = TaskImportance.Low,
            TaskState = TaskState.Late,
            Timer = new TimeSpan(),
        };
        
        taskDataService.AddTask(newTask);
        LoadTasks();
    }

    public void UpdateTask(Task updateTask)
    {
        taskDataService.UpdateTask(updateTask);
        LoadTasks();
    }

    public void DeleteTask(int taskId)
    {
        taskDataService.DeleteTasks(taskId);
        LoadTasks();
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    public TaskViewModel()
    {
        taskDataService = new TaskDataService();
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}