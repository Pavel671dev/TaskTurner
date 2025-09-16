using System.Windows;
using System.Windows.Controls;
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
        DataContext = new MainWindowViewModel();
    }
    
    
}