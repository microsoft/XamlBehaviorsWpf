using System.Windows;
using System.Windows.Controls;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Interaction logic for ConditionPage.xaml
    /// </summary>
    public partial class ConditionPage : UserControl
    {
        private CallMethodControl _callMethodControl;
        private ChangePropertyControl _changePropertyControl;
        private GoToStateControl _goToStateControl;
        private InvokeCommandControl _invokeCommandControl;
        private LaunchUriControl _launchUriControl;
        private PlaySoundControl _playSoundControl;

        public ConditionPage()
        {
            InitializeComponent();
            this._callMethodControl = new CallMethodControl();
            this._changePropertyControl = new ChangePropertyControl();
            this._goToStateControl = new GoToStateControl();
            this._invokeCommandControl = new InvokeCommandControl();
            this._launchUriControl = new LaunchUriControl();
            this._playSoundControl = new PlaySoundControl();
        }

        private void CallMethodButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._callMethodControl);
        }

        private void ChangePropertyButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._changePropertyControl);
        }

        private void GoToStateButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._goToStateControl);
        }

        private void InvokeCommandButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._invokeCommandControl);
        }

        private void LaunchUriButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._launchUriControl);
        }

        private void PlaySoundButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(this._playSoundControl);
        }

        private void RemoveElementButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainContent.Children.Clear();
            this.MainContent.Children.Add(new RemoveElementControl());
        }
    }
}
