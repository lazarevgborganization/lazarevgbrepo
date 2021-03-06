﻿using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TestForSCAUT_v_2.Commands
{
    /// <summary>
    /// Этот класс позволяет делегировать логику команд методам, передаваемым в качестве параметров, 
    /// и привязывать команды к объектам, не принадлежащим дереву элемента
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action _executeMethod;
        private readonly Func<bool> _canExecuteMethod;
        private bool _isAutomaticRequeryDisabled;
        private List<WeakReference> _canExecuteChangedHandlers;

        #region Constructors
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Определяет, может ли быть исполнена команда
        /// </summary>
        public bool CanExecute()
        {
            if (_canExecuteMethod != null)
                return _canExecuteMethod();
            return true;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        public void Execute()
        {
            if (_executeMethod != null)
                _executeMethod();
        }

        /// <summary>
        /// Вызов события CanExecuteChanged
        /// </summary>
        public void RaiseCanExecuteChanged()
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
                if (_isAutomaticRequeryDisabled != value)
                {
                    if (value)
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    else
                        CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                    _isAutomaticRequeryDisabled = value;
                }
            }
        }
        #endregion

        #region ICommand Members
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

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
        #endregion
    }
}
