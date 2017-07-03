using System.ComponentModel;
using System.Windows.Controls;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for CallMethodAction.xaml
    /// </summary>
    public partial class CallMethodControl : UserControl, INotifyPropertyChanged
    {
        public int Count { get; set; }

        public CallMethodControl()
        {
            InitializeComponent();
            this.DataContext = this;
            Count = 0;
        }

        public void IncrementCount()
        {
            Count++;
            OnPropertyChanged(nameof(Count));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
