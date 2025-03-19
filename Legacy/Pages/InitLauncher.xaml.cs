using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Arcane_Launcher.Responses.Lightswitch;
using Arcane_Launcher.Utils;

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
                    Thread.Sleep(1000);
                    LoadingText.Text = status.Message;
                    Thread.Sleep(1000);
                    LoadingText.Text = "AppName: " + status.LauncherInfoDTO.AppName;
                    if (Window.GetWindow(this) is Window window)
                    {
                        window.Title = "Launcher - " + status.LauncherInfoDTO.AppName;
                    }
                    Thread.Sleep(1000);
                    Utils.Globals.MainFrame.Navigate(new Pages.Auth.Login());
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
                if ((int)response.StatusCode != 200)
                {
                    Utils.Logger.warn("got bad response from " + url + " : " + await response.Content.ReadAsStringAsync());
                    return null;
                }
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Utils.Logger.good("Successfully got response from " + url + " : " + responseBody);

                LightswitchStatus status = JsonConvert.DeserializeObject<LightswitchStatus>(responseBody);

                Utils.Logger.good($"Successfully got status: {status?.Status}");
                Utils.Logger.good($"Message: {status?.Message}");
                Utils.Logger.good($"Deserialized App Name: {status?.LauncherInfoDTO?.AppName}");

                return status;
            } catch
            {
                Utils.Logger.warn($"Could not get service status!");
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