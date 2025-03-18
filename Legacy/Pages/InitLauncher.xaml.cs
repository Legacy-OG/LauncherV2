using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Arcane_Launcher.Responses.Lightswitch;

namespace Arcane_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for InitLauncher.xaml
    /// </summary>
    public partial class InitLauncher : Page
    {
        private string domain = "http://legacy-service-prod.ol.evolvefn.com:3551";
        private static readonly HttpClient httpClient = new HttpClient();

        public InitLauncher()
        {
            InitializeComponent();
            SetupLauncher();
        }

        private async Task SetupLauncher()
        {
            LightswitchStatus status = await GetLightswitchStatusAsync();
            if (status != null)
            {
                if (status.Status == "UP")
                {
                    LoadingText.Text = "Status: UP";
                    Thread.Sleep(1500);
                    LoadingText.Text = status.Message;
                    Thread.Sleep(1500);
                    LoadingText.Text = "AppName: " + status.LauncherInfoDTO.AppName;
                    if (Window.GetWindow(this) is Window window)
                    {
                        window.Title = "Launcher - " + status.LauncherInfoDTO.AppName;
                    }
                    Thread.Sleep(1500);
                }
                else
                {
                    ShowErrorOverlay("Launcher Down!", "Sorry, the launcher is currently down for maintainance, Check back later!");
                }
            }
            else
            {
                ShowErrorOverlay("Error", "Failed to get launcher status");
            }
        }

        private async Task<LightswitchStatus> GetLightswitchStatusAsync()
        {
            try
            {
                string url = $"{domain}/lightswitch/api/service/launcher/status";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);

                LightswitchStatus status = JsonConvert.DeserializeObject<LightswitchStatus>(responseBody);

                Console.WriteLine($"Deserialized Status: {status?.Status}");
                Console.WriteLine($"Deserialized Message: {status?.Message}");
                Console.WriteLine($"Deserialized App Name: {status?.LauncherInfoDTO?.AppName}");

                return status;
            } catch
            {
                return null;
            }
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