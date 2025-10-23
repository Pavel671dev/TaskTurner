using System.Windows.Input;

namespace TaskTurner.ViewModel;

public class RelayCommand : ICommand
{
    private readonly Func<bool> canExecute;
    private readonly Action execute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return canExecute == null || true;
    }

    public void Execute(object parameter)
    {
        execute();
    }
}