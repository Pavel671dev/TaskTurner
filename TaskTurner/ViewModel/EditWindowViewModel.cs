using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TaskTurner.DataServices;
using TaskTurner.Models;

namespace TaskTurner.ViewModel;

public class EditWindowViewModel : INotifyPropertyChanged
{
    private readonly TaskDataService taskDataService;

    public EditWindowViewModel()
    {
        taskDataService = new TaskDataService(Environment
            .GetFolderPath(Environment.SpecialFolder.ApplicationData));
        // Resource for Combobox
        Importances = InitializeTaskImportance();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StartDate { get; set; }
    public TaskImportance TaskImportance { get; set; }
    public ObservableCollection<Subtask> TaskCheckList { get; set; }

    public Subtask SelectedSubtask { get; set; }

    public string Subtask { get; set; }

    public ObservableCollection<TaskImportance> Importances { get; set; }

    public ICommand EditTaskCommand => new RelayCommand(EditTask);

    public ICommand AddNewSubtaskCommand => new RelayCommand(AddNewSubtask);

    public ICommand DeleteSubtaskCommand => new RelayCommand(DeleteSubtask);

    public Action CloseAction { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<TaskImportance> InitializeTaskImportance()
    {
        return new ObservableCollection<TaskImportance>(Enum
            .GetValues(typeof(TaskImportance))
            .Cast<TaskImportance>());
    }

    private void EditTask()
    {
        var tasks = taskDataService.LoadTasks();
        foreach (var task in tasks.Where(task => task.Id == Id))
        {
            task.Title = Title;
            task.Description = Description;
            task.TaskCheckList = TaskCheckList;
            task.DueDate = DueDate;
            taskDataService.UpdateTask(task);
            break;
        }

        taskDataService.LoadTasks();
        CloseAction();
    }

    private void AddNewSubtask()
    {
        if (Subtask == null)
        {
            return;
        }
        TaskCheckList.Add(new Subtask(Subtask));
        Subtask = null;

        OnPropertyChanged(nameof(Subtask));
    }

    private void DeleteSubtask()
    {
        TaskCheckList.Remove(SelectedSubtask);
        SelectedSubtask = null;
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}