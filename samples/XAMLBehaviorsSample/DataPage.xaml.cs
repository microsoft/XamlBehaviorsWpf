using System.Windows;
using System.Windows.Controls;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : UserControl
    {
        private DataStateControl _dataStateControl;
        private SetDataValueControl _setDataValueControl;

        public DataPage()
        {
            InitializeComponent();
            this._dataStateControl = new DataStateControl();
            this._setDataValueControl = new SetDataValueControl();
        }

        private void DataStateBehavior_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._dataStateControl);
        }

        private void SetDataStoreValueAction_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._setDataValueControl);
        }
    }
}
