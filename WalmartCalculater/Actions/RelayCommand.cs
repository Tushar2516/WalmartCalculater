using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace WalmartCalculater.Actions
{
    public class RelayCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action DoWork;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public RelayCommand(Action Work)
        {
            DoWork = Work;
        }
        public void Execute(object parameter)
        {
            DoWork();
        }
    }
}
