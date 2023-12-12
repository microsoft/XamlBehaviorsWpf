using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for InvokeCommandControl.xaml
    /// </summary>
    public partial class InvokeCommandControl : UserControl
    {
        public ChangeColorCommand ColorCommand { get; private set; }

        public InvokeCommandControl()
        {
            this.ColorCommand = new ChangeColorCommand(this);
            InitializeComponent();
            this.DataContext = this;
        }
    }

    public class ChangeColorCommand : ICommand
    {
        private InvokeCommandControl control;

#pragma warning disable CS0067 // The event is never used
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event is never used

        public ChangeColorCommand(InvokeCommandControl control)
        {
            this.control = control;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Brush currentBackground = (Brush)parameter;

            if (currentBackground != Brushes.DarkBlue)
            {
                this.control.Grid.Background = Brushes.DarkBlue;
            } else
            {
                this.control.Grid.Background = Brushes.DeepPink;
            }
        }
    }
}
