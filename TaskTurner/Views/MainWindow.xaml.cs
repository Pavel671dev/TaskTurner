using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TaskTurner.ViewModel;
using Task = TaskTurner.Models.Task;
using TaskTurner.DataServices;

namespace TaskTurner;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Task selectedTask { get; set; }
    private TaskDataService taskDataService { get; }

    private MainWindowViewModel viewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        taskDataService = new TaskDataService();
        TaskListView.Items.Filter = FilterByName;

    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TaskSearchBox.Text != null)
        {
            TaskListView.Items.Filter = FilterByName;
        };
        selectedTask = (Task)TaskListView.SelectedItem;
        if (selectedTask is null) return;
        TaskTitle.Text = selectedTask.Title;
        TaskDescription.Text = selectedTask.Description;
        TaskDueDate.Text = "Due: " + selectedTask.DueDate.ToShortDateString();
        TaskCheckListView.ItemsSource = TakeSubtasks();

        TaskImportance.Background = SetImportanceColor();
    }

    private ObservableCollection<string> TakeSubtasks()
    {
        var subtasks = selectedTask.TaskCheckList;             
        var subtasksText = new ObservableCollection<string>(); 
        foreach (var subtask in subtasks)                  
        {                                                  
            subtasksText.Add(subtask.Description);         
        }
        return subtasksText;
    }

    private SolidColorBrush SetImportanceColor()
    {
        switch (selectedTask.TaskImportance)                                         
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
        Task task = (Task)filterTask;
        return (task.Title.Contains(TaskSearchBox.Text, StringComparison.OrdinalIgnoreCase) && !task.IsCompleted);
    }

    private void TaskSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (TaskSearchBox.Text != null)
        {
            TaskListView.Items.Filter = FilterByName;
        }
    }
}