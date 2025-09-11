using System.Windows;
using TaskTurner.ViewModel;

namespace TaskTurner;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.DataContext = new TaskViewModel();
    }
}