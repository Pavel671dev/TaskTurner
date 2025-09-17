using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using TaskTurner.Views;
using TaskTurner.DataServices;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly TaskDataService taskDataService;

    private ObservableCollection<Task> tasks { get; set; }

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
    public MainWindowViewModel()
    {
        taskDataService = new TaskDataService();
        LoadTasks();
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);

    public ICommand IDeleteTaskCommand => new RelayCommand(DeleteTaskAction);

    private void DeleteTaskAction()
    {
        throw new NotImplementedException();
    }

    private void OpenNewWindow()
    {
        NewTaskWindow newTaskWindow = new NewTaskWindow();
        newTaskWindow.Show();
    }
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}