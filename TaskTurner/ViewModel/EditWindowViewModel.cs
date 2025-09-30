using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TaskTurner.DataServices;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;
using TaskTurner.ViewModel;


namespace TaskTurner.ViewModel;

public class EditWindowViewModel: INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StartDate { get; set; }
    public TaskImportance TaskImportance { get; set; }
    public ObservableCollection<Subtask> TaskCheckList { get; set; }
    
    public Subtask SelectedSubtask { get; set; }

    private TaskDataService taskDataService;
    
    public string Subtask { get; set; }

    public ObservableCollection<TaskImportance> Importances { get; set; }
    
    public ObservableCollection<TaskImportance> InitializeTaskImportance()
    {
        return new ObservableCollection<TaskImportance>(Enum.GetValues(typeof(TaskImportance)).Cast<TaskImportance>());
    }

    public ICommand IEditTask => new RelayCommand(EditTask);
    
    public ICommand IAddNewSubtask => new RelayCommand(AddNewSubtask);

    public ICommand IDeleteSubtask => new RelayCommand(DeleteSubtask);
    
    public Action CloseAction { get; set; }
    public EditWindowViewModel()
    {
        taskDataService = new TaskDataService();
        TaskCheckList = new ObservableCollection<Subtask>();
        // Resource for Combobox
        Importances = InitializeTaskImportance();
    }

    private void EditTask()
    {
        List<Task> tasks = taskDataService.LoadTasks();
        foreach (var task in tasks)
        {
            if (task.Id == Id)
            {
                task.Title = Title;
                task.Description = Description;
                task.TaskCheckList = TaskCheckList;
                task.DueDate = DueDate;
                taskDataService.UpdateTask(task);
                break;
            }
        }
        
        taskDataService.LoadTasks();
        CloseAction();
    }
    
    public void AddNewSubtask()
    {
        if (Subtask == null) return;
        TaskCheckList.Add(new Subtask(Subtask));
        Subtask = null;
        
        OnPropertyChanged(nameof(Subtask));
    }

    private void DeleteSubtask()
    {
        TaskCheckList.Remove(SelectedSubtask);
        SelectedSubtask = null;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}