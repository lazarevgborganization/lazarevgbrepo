using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestForSCAUT_v_2.Commands;
using TestForSCAUT_v_2.ViewModels;
using TestForSCAUT_v_2.Models;
using System.Windows;
using System.Windows.Input;

namespace TestForSCAUT_v_2.ViewModels
{
    public class MainViewModel
    {
        TerminalDataSource tds = new TerminalDataSource();
        private DelegateCommand chooseFileCommand;
        private DelegateCommand clearDisplayCommand;
        private DelegateCommand exitCommand;
        private ObservableCollection<TerminalViewModel> terminals = new ObservableCollection<TerminalViewModel>();

        public ObservableCollection<TerminalViewModel> Terminals
        {
            get { return terminals; }
        }

        public ICommand ChooseFileCommand
        {
            get
            {
                if (chooseFileCommand == null)
                {
                    chooseFileCommand = new DelegateCommand(ChooseFile, CanChooseFile);
                }
                return chooseFileCommand;
            }
        }

        public ICommand ClearDisplayCommand
        {
            get
            {
                if (clearDisplayCommand == null)
                {
                    clearDisplayCommand = new DelegateCommand(ClearDisplay, CanClearDisplay);
                }
                return clearDisplayCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(Exit);
                }
                return exitCommand;
            }
        }

        private bool CanChooseFile()
        {
            return tds.Load() != null;
        }

        private void ChooseFile()
        {
            tds.ReadFile(tds.Load());
        }

        private bool CanClearDisplay()
        {
            return terminals.Count > 0;
        }

        private void ClearDisplay()
        {
            terminals.Clear();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
