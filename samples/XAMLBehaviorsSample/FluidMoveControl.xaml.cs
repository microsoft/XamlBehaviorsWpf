using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for FluidMoveControl.xaml
    /// </summary>
    public partial class FluidMoveControl : UserControl
    {
        public FluidMoveControl()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            rect.Fill = Brushes.DeepPink;
            rect.Margin = new Thickness(5.0);
            this.Panel.Children.Add(rect);
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Panel.Children.Count > 0)
            {
                this.Panel.Children.RemoveAt(0);
            }
        }
    }
}
