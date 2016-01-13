using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestForSCAUT_v_2.Commands
{
    /// <summary>
    /// This class contains methods for the CommandManager that help avoid memory leaks by
    ///  using weak references.
    /// </summary>
    internal class CommandManagerHelper
    {
        /// <summary>
        /// Вызывает обработчики слабых ссылок
        /// </summary>
        /// <param name="handlers"></param>
            internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
            {
            if (handlers != null)
            {
                // Сохраняем данные об обработчиках перед их вызовом, т.к. они могут служить причиной
                //модификации читаемого массива
                EventHandler[] callers = new EventHandler[handlers.Count];
                int count = 0;

                for (int i = handlers.Count; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler handler = reference.Target as EventHandler;
                    if (handler == null)
                    {
                        //Убираем обработчики из списка обработчиков, собранные сборщиком мусора
                        handlers.RemoveAt(i);
                    }
                    else
                    {
                        callers[count] = handler;
                        count++;
                    }
                }

                //Вызов обработчиков, данные о которых были сохранены
                for (int i = handlers.Count; i >= 0; i--)
                {
                    EventHandler handler = callers[i];
                    handler(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Подписка на событие CommandManager.RequerySuggested 
        /// (Возникает, если CommandManager обнаруживает условия, 
        /// которые могут изменить возможность выполнения команды.)
        /// </summary>
        /// <param name="handlers"></param>
        internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                foreach (WeakReference handlerRef in handlers)
                {
                    EventHandler handler = handlerRef.Target as EventHandler;
                    if (handler != null)
                        //Возникает, если обнаружены условия, способные повлиять на возможность выполнения команды
                        CommandManager.RequerySuggested += handler;
                }
            }
        }

        /// <summary>
        /// Отписка от события CommandManager.RequerySuggested
        /// (Возникает, если CommandManager обнаруживает условия, 
        /// которые могут изменить возможность выполнения команды.)
        /// </summary>
        /// <param name="handlers"></param>
        internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                foreach (WeakReference handlerRef in handlers)
                {
                    EventHandler handler = handlerRef.Target as EventHandler;
                    if (handler != null)
                        CommandManager.RequerySuggested -= handler;
                }
            }
        }

        /// <summary>
        /// Добавляет обработчик в передаваемый список обработчиков, используя слабую ссылку.
        /// Размер списка по умолчанию: -1.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="handler"></param>
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        /// <summary>
        ///  Добавляет обработчик в передаваемый список обработчиков, используя слабую ссылку.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="handler"></param>
        /// <param name="defaultSize"></param>
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler,
                                                     int defaultSize)
        {
            if (handlers != null)
            {
                handlers = defaultSize > 0 ? new List<WeakReference>(defaultSize) : new List<WeakReference>(0);
            }

            handlers.Add(new WeakReference(handler));
        }
        /// <summary>
        ///  Удаляет обработчик из передаваемого списка обработчиков
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="handler"></param>
        internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        //Убираем из списка обработчики ,собранные сборщиком мусора, в дополнение к обработчику, 
                        //уже удаленному
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}
