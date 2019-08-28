using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BITClientServer.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> localAction;
        private bool _localCanExecute;

        public RelayCommand(Action<T> action, bool canExecute)
        {
            localAction = action;
            _localCanExecute = canExecute;
        }

#pragma warning disable CS0067 // The event 'RelayCommand<T>.CanExecuteChanged' is never used
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event 'RelayCommand<T>.CanExecuteChanged' is never used

        public bool CanExecute(object parameter)
        {
            return _localCanExecute;
        }

        public void Execute(Object parameter)
        {
            localAction((T)parameter);
        }
    }
}
