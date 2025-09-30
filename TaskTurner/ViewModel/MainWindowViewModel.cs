using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TaskTurner.DataServices;
using TaskTurner.Models;
using TaskTurner.Views;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly TaskDataService taskDataService;

    public MainWindowViewModel()
    {
        taskDataService = new TaskDataService();
        LoadTasks();
    }

    private ObservableCollection<Task> tasks { get; set; }

    public Task SelectedTask { get; set; }

    public ObservableCollection<Subtask> Subtasks { get; set; }

    public ObservableCollection<Task> Tasks
    {
        get => tasks;
        set
        {
            tasks = value;
            OnPropertyChanged(nameof(Tasks));
        }
    }

    public ICommand IOpenNewWindowCommand => new RelayCommand(OpenTaskAddWindow);

    public ICommand IDeleteTaskCommand => new RelayCommand(DeleteTask);

    public ICommand ICompleteTaskCommand => new RelayCommand(CompleteTask);

    public ICommand IEditTaskCommand => new RelayCommand(EditTask);

    public event PropertyChangedEventHandler PropertyChanged;

    public void LoadTasks()
    {
        var TaskList = taskDataService.LoadTasks();
        Tasks = new ObservableCollection<Task>(TaskList);
    }

    private void DeleteTask()
    {
        if (!IsClickable()) return;
        var result = MessageBox.Show("Are you sure want to delete this task?",
            "Warning", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            taskDataService.DeleteTasks(SelectedTask.Id);
            LoadTasks();
            SelectedTask = null;
        }
    }

    private void CompleteTask()
    {
        if (!IsClickable()) return;
        var completedTask = SelectedTask;
        completedTask.IsCompleted = true;
        taskDataService.UpdateTask(completedTask);
        LoadTasks();
        SelectedTask = null;
    }

    private void OpenTaskAddWindow()
    {
        var newTaskWindow = new NewTaskWindow();
        newTaskWindow.Owner = Application.Current.MainWindow;
        newTaskWindow.Show();
    }

    private void EditTask()
    {
        if (!IsClickable()) return;
        var editWindow = new EditWindow(SelectedTask);
        editWindow.EditedTask = SelectedTask;
        editWindow.Owner = Application.Current.MainWindow;
        editWindow.Show();
    }

    private bool IsClickable()
    {
        return SelectedTask != null;
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}