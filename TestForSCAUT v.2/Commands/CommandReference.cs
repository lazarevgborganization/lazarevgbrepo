using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestForSCAUT_v._2.Commands
{
    /// <summary>
    /// Данный класс упрощает взаимодействие ключа, забинденного в XAML разметке, и команды, определенной в ViewModel,
    /// путем использования свойства Command dependency. Унаследован от Freezable для устранения ограничений в WPF 
    /// в виду привязки данных в XAML.
    /// </summary>
    public class CommandReference :  Freezable, ICommand
    {
        /// <summary>
        /// Свойство зависимости
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(CommandReference),
            new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandReference comRef = d as CommandReference;
            ICommand oldCommand = e.OldValue as ICommand;
            ICommand newCommand = e.NewValue as ICommand;

            if(oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= comRef.CanExecuteChanged;
            }
            if(newCommand != null)
            {
                newCommand.CanExecuteChanged += comRef.CanExecuteChanged;
            }
        }
        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(Command != null)
            {
                return Command.CanExecute(parameter);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if(Command != null)
            {
                Command.Execute(parameter);
            }
        }
        #endregion

        #region Freezable Members
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
