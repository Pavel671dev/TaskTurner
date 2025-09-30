using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TaskTurner.DataServices;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.ViewModel;

public class TaskCreationViewModel : INotifyPropertyChanged
{
    private readonly TaskDataService taskDataService;

    private ObservableCollection<Task> tasks;

    public TaskCreationViewModel()
    {
        taskDataService = new TaskDataService();
        TaskCheckList = new ObservableCollection<Subtask>();
        DueDate = DateTime.Now;
        // Resource for Combobox
        Importances = InitializeTaskImportance();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsCompleted { get; set; }
    public TaskImportance TaskImportance { get; set; }

    public string SubTask { get; set; }

    public ObservableCollection<TaskImportance> Importances { get; set; }


    public ObservableCollection<Subtask> TaskCheckList { get; set; }

    public ICommand IAddNewTask => new RelayCommand(AddNewTask);
    public ICommand IAddNewSubtask => new RelayCommand(AddNewSubtask);

    public ObservableCollection<Task> Tasks
    {
        get => tasks;
        set
        {
            tasks = value;
            OnPropertyChanged(nameof(Tasks));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void LoadTasks()
    {
        var TaskList = taskDataService.LoadTasks();
        Tasks = new ObservableCollection<Task>(TaskList);
    }

    public void AddNewTask()
    {
        if (!IsAddableTitle(Title))
        {
            MessageBox.Show("The first character of title must be a letter", "Warning");
            return;
        }

        var newTask = new Task
        {
            Title = Title,
            Description = Description,
            Id = taskDataService.GenerateNewTaskID(),
            DueDate = DueDate,
            IsCompleted = false,
            StartDate = DateTime.Now,
            TaskCheckList = TaskCheckList,
            TaskImportance = TaskImportance,
            TaskState = TaskState.Late
        };

        taskDataService.AddTask(newTask);

        ClearFields();
        LoadTasks();
    }

    private bool IsAddableTitle(string title)
    {
        return char.IsLetter(title[0]);
    }

    public void AddNewSubtask()
    {
        if (SubTask == null) return;
        TaskCheckList.Add(new Subtask(SubTask));
        SubTask = null;

        OnPropertyChanged(nameof(SubTask));
    }

    private void ClearFields()
    {
        Title = "";
        Description = "";
        DueDate = DateTime.Now;
        TaskCheckList.Clear();
        UpdateWindow();
    }

    private void UpdateWindow()
    {
        OnPropertyChanged(Title);
        OnPropertyChanged(Description);
        OnPropertyChanged(nameof(DueDate));
        OnPropertyChanged(nameof(TaskCheckList));
        OnPropertyChanged(nameof(Importances));
    }

    public ObservableCollection<TaskImportance> InitializeTaskImportance()
    {
        return new ObservableCollection<TaskImportance>(Enum.GetValues(typeof(TaskImportance)).Cast<TaskImportance>());
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}