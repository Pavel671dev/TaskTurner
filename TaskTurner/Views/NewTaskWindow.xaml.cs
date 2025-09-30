using System.Windows;
using TaskTurner.ViewModel;

namespace TaskTurner.Views;

public partial class NewTaskWindow : Window
{
    public event EventHandler<string> AfterClosingEvent;
    public NewTaskWindow()
    {
        InitializeComponent();

        DataContext = new TaskCreationViewModel();
        this.Closing += OnClosed;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        this.Owner.DataContext = DataContext;
    }
}