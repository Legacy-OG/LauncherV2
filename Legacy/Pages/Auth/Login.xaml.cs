using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arcane_Launcher.Pages.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            AuthorizationCodeBox.TextChanged += AuthorizationCodeBox_TextChanged;
        }

        private void AuthorizationCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthorizationCodeBox.Text))
            {
                LoginButton.Background = new SolidColorBrush(Color.FromRgb(68, 67, 72));
            }
            else
            {
                LoginButton.Background = new SolidColorBrush(Color.FromRgb(38, 187, 255));
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ShowErrorOverlay(string errorTitle, string errorMessage)
        {
            Dispatcher.Invoke(() =>
            {
                ErrorTitle.Text = errorTitle;
                ErrorMessage.Text = errorMessage;
                ErrorOverlay.Visibility = Visibility.Visible;
            });
        }

        private void CloseErrorOverlay(object sender, RoutedEventArgs e)
        {
            ErrorOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
