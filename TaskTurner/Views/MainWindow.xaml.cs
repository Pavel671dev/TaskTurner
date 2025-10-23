using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TaskTurner.ViewModel;
using Task = TaskTurner.Models.Task;

namespace TaskTurner;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        TaskListView.Items.Filter = FilterByName;
    }

    private Task SelectedTask { get; set; }

    private MainWindowViewModel ViewModel { get; }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TaskSearchBox.Text != null)
        {
            TaskListView.Items.Filter = FilterByName;
        }
        SelectedTask = (Task)TaskListView.SelectedItem;
        if (SelectedTask is null)
        {
            return;
        }
        TaskTitle.Text = SelectedTask.Title;
        TaskDescription.Text = SelectedTask.Description;
        TaskDueDate.Text = "Due: " + SelectedTask.DueDate.ToShortDateString();
        TaskCheckListView.ItemsSource = TakeSubtasks();

        TaskImportance.Background = SetImportanceColor();
    }

    private ObservableCollection<string> TakeSubtasks()
    {
        var subtasks = SelectedTask.TaskCheckList;
        var subtasksText = new ObservableCollection<string>();
        foreach (var subtask in subtasks)
        {
            subtasksText.Add(subtask.Description);
        }
        return subtasksText;
    }

    private SolidColorBrush SetImportanceColor()
    {
        switch (SelectedTask.TaskImportance)
        {
            case Models.TaskImportance.Low:
                return new SolidColorBrush(Colors.ForestGreen);
            case Models.TaskImportance.Medium:
                return new SolidColorBrush(Colors.Yellow);
            case Models.TaskImportance.High:
                return new SolidColorBrush(Colors.DarkRed);
            default:
                return new SolidColorBrush(Colors.ForestGreen);
        }
    }

    private bool FilterByName(object filterTask)
    {
        var task = (Task)filterTask;
        return task.Title.Contains(TaskSearchBox.Text, StringComparison.OrdinalIgnoreCase) && !task.IsCompleted;
    }

    private void TaskSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (TaskSearchBox.Text != null)
        {
            TaskListView.Items.Filter = FilterByName;
        }
    }
}