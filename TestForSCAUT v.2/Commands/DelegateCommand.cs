using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestForSCAUT_v._2.Commands
{
    class DelegateCommand : ICommand
    {
        private readonly Action _executeMethod;
        private readonly Func<bool> _canExecuteMethod;
        private bool _isAutomaticRequeryDisabled;
        private List<WeakReference> _canExecuteChangedHandlers;


        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false)
        {
        }
        
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod,canExecuteMethod,false)
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
        

       public event EventHandler CanExecuteChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public bool CanExecute()
        {
            if (_canExecuteMethod != null)
                return _canExecuteMethod();
            return true;
        }

        public void Execute()
        {
            if (_executeMethod != null)
                _executeMethod();
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

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
                    if (value)
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    else
                        CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                    _isAutomaticRequeryDisabled = value;
                }
            }
        }

        internal class CommandManagerHelper
        {
            internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
            {
                if (handlers != null)
                {                     
                    // Take a snapshot of the handlers before we call out to them since the handlers
                    // could cause the array to me modified while we are reading it.
                    EventHandler[] callers = new EventHandler[handlers.Count];
                    int count = 0;

                    for(int i=handlers.Count;i>=0;i--)
                    {
                        WeakReference reference = handlers[i];
                        EventHandler handler = reference.Target as EventHandler;
                        if(handler == null)
                        {
                            // Clean up old handlers that have been collected
                            handlers.RemoveAt(i);
                        }
                        else
                        {
                            callers[count] = handler;
                            count++;
                        }
                    }

                    // Call the handlers that we snapshotted
                    for (int i=handlers.Count;i>=0;i--)
                    {
                        EventHandler handler = callers[i];
                        handler(null, EventArgs.Empty);
                    }
                }
            }

            internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers)
            {
                if(handlers != null)
                {
                    foreach(WeakReference handlerRef in handlers)
                    {
                        EventHandler handler = handlerRef.Target as EventHandler;
                        if (handler != null)
                            CommandManager.RequerySuggested += handler;
                    }
                }
            }

            internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers)
            {
                if(handlers != null)
                {
                    foreach(WeakReference handlerRef in handlers)
                    {
                        EventHandler handler = handlerRef.Target as EventHandler;
                        if (handler != null)
                            CommandManager.RequerySuggested -= handler;
                    }
                }
            }

            internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
            {
                AddWeakReferenceHandler(ref handlers, handler, -1);
            }

            internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler,
                                                         int defaultSize)
            {
                if (handlers != null)
                {
                    handlers = defaultSize > 0 ? new List<WeakReference>(defaultSize) : new List<WeakReference>(0);
                }

                handlers.Add(new WeakReference(handler));
            }

            internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
            {
                if(handlers != null)
                {
                    for(int i=handlers.Count - 1;i>=0;i--)
                    {
                        WeakReference reference = handlers[i];
                        EventHandler existingHandler = reference.Target as EventHandler;
                        if ((existingHandler == null) || (existingHandler == handler))
                        {
                            // Clean up old handlers that have been collected
                            // in addition to the handler that is to be removed.
                            handlers.RemoveAt(i);
                        }      
                    }
                }
            }
        }
    }
}
