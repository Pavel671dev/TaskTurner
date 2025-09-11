using System.ComponentModel;
using System.Windows.Input;
using TaskTurner.Views;

namespace TaskTurner.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);

    private void OpenNewWindow()
    {
        NewTaskWindow newTaskWindow = new NewTaskWindow();
        newTaskWindow.Show();
    }
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}