using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace _3D66.Download.Tools
{
    public class EasyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<object> CallBack { get; set; }
        public EasyCommand(Action<object> cb)
        {
            this.CallBack = cb;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CallBack?.Invoke(parameter);
        }
    }
}
