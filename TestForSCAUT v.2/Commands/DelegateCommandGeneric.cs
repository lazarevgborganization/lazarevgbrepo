using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TestForSCAUT_v_2.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod = null;
        private readonly Func<T, bool> _canExecuteMethod = null;
        private bool _isAutomaticRequeryDisabled = false;
        private List<WeakReference> _canExecuteChangedHandlers;

        #region Constructors
        public DelegateCommand(Action<T> executeMethod)
            :this(executeMethod,null,false)
            {
            }
        public DelegateCommand(Action<T> executeMethod, Func<T,bool> canExecuteMethod)
            :this(executeMethod,canExecuteMethod,false)
        {
        }
        public DelegateCommand(Action<T> executeMethod, Func<T,bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if(executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Определяет, может ли быть исполнена команда
        /// </summary>
        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            return true;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        public void Execute(T parameter)
        {
            if (_executeMethod != null)
                _executeMethod(parameter);
        }

        /// <summary>
        ///  Проверяет ,включен ли автоматический перезапрос команды
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get
            {
                return _isAutomaticRequeryDisabled;
            }
            set
            {
                if(_isAutomaticRequeryDisabled != value)
                {
                    if(value)
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    }
                    _isAutomaticRequeryDisabled = value;
                }
            }
        }

        /// <summary>
        /// Вызов события CanExecuteChanged
        /// </summary>
        public void RaiseCanExectuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        ///  Защищенный виртуальный метод для вызова события CanExecuteChanged
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Происходит при возникновении изменений, влияющих на то, должна ли выполняться данная команда. 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if(!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }

            remove
            {
                if(!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if((parameter == null) && typeof(T).IsValueType)
            {
                return (_canExecuteMethod == null);
            }
            return _canExecuteMethod((T)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }
        #endregion
    }
}
