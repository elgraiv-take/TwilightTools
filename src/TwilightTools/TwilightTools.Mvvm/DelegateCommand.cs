using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Elgraiv.TwilightTools.Mvvm
{

    public class DelegateCommand : ICommand
    {

        private Action _action;
        private Func<bool>? _predicate;
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action action) : this(action, null)
        {

        }

        public DelegateCommand(Action action, Func<bool>? predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public void RaiseCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return _predicate?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _action?.Invoke();
        }
    }

    public class DelegateCommand<T> : ICommand where T:notnull
    {
        private Action<T> _action;
        private Func<T, bool>? _predicate;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<T> action) : this(action, null)
        {

        }

        public DelegateCommand(Action<T> action, Func<T, bool>? predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public void RaiseCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter is T param)
            {
                return _predicate?.Invoke(param) ?? true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Execute(object? parameter)
        {
            if (parameter is T param)
            {
                _action?.Invoke(param);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
