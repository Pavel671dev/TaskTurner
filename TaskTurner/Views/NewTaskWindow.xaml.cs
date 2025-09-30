using System.Windows;
using TaskTurner.ViewModel;

namespace TaskTurner.Views;

public partial class NewTaskWindow : Window
{
    public NewTaskWindow()
    {
        InitializeComponent();

        DataContext = new TaskCreationViewModel();
        Closing += OnClosed;
    }

    public event EventHandler<string> AfterClosingEvent;

    private void OnClosed(object? sender, EventArgs e)
    {
        Owner.DataContext = DataContext;
    }
}