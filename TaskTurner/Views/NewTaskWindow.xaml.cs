using System.Windows;
using TaskTurner.ViewModel;

namespace TaskTurner.Views;

public partial class NewTaskWindow : Window
{
    public NewTaskWindow()
    {
        InitializeComponent();

        DataContext = new TaskViewModel();
    }
}