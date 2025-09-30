using System.Windows;
using TaskTurner.Models;
using TaskTurner.DataServices;
using TaskTurner.ViewModel;
using Task = TaskTurner.Models.Task;

namespace TaskTurner.Views;

public partial class EditWindow : Window
{
    public Task EditedTask;
    
    private TaskDataService taskDataService;
    
    public EditWindow(Task selectedTask)
    {
        InitializeComponent();
        EditWindowViewModel editWindowViewModel = new EditWindowViewModel();
        DataContext = editWindowViewModel;
        taskDataService = new TaskDataService();
        
        EditedTask = selectedTask;
        TitleBox.Text =  EditedTask.Title;
        Description.Text = EditedTask.Description;
        Date.SelectedDate =  EditedTask.DueDate;
        SubtaskView.ItemsSource = EditedTask.TaskCheckList;
        
        this.Closing += OnClosed;
        if (editWindowViewModel.CloseAction == null)
        {
            editWindowViewModel.CloseAction = Close;
        }
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        this.Owner.DataContext = DataContext;
    }
}