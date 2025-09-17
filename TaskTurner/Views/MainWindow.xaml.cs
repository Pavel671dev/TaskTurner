using System.Windows;
using System.Windows.Controls;
using TaskTurner.ViewModel;
using TaskTurner.Models;
using Task = TaskTurner.Models.Task;
using TaskTurner.DataServices;

namespace TaskTurner;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public Task selectedTask { get; set; }
    private TaskDataService taskDataService { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        taskDataService = new TaskDataService();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedTask = (Task)TaskListView.SelectedItem;
        TaskTitle.Text = selectedTask.Title;
        TaskDescription.Text = selectedTask.Description;
        TaskDueDate.Text = selectedTask.DueDate.ToShortDateString();
    }

    private void DeleteTaskAction(object sender)
    {
        taskDataService.DeleteTasks(selectedTask.Id);
        selectedTask = null;
    }
}