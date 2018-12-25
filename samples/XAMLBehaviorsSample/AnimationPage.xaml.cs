using System.Windows;
using System.Windows.Controls;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for AnimationPage.xaml
    /// </summary>
    public partial class AnimationPage : UserControl
    {
        private ControlStoryboardControl _controlStoryboard;
        private FluidMoveControl _fluidMoveControl;
        private FluidMoveSetTagControl _fluidMoveSetTagControl;
        private MouseDragElementControl _mouseDragElementControl;
        private TouchBehaviorControl _touchBehaviorControl;

        public AnimationPage()
        {
            this._controlStoryboard = new ControlStoryboardControl();
            this._fluidMoveControl = new FluidMoveControl();
            this._fluidMoveSetTagControl = new FluidMoveSetTagControl();
            this._mouseDragElementControl = new MouseDragElementControl();
            this._touchBehaviorControl = new TouchBehaviorControl();
            InitializeComponent();
        }

        private void ControlStoryboardAction_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._controlStoryboard);
        }

        private void FluidMoveBehavior_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._fluidMoveControl);
        }

        private void FluidMoveSetTagBehavior_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._fluidMoveSetTagControl);
        }

        private void MouseDragElementBehavior_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._mouseDragElementControl);
        }

        private void TranslateZoomRotateBehavior_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._touchBehaviorControl);
        }
    }
}
