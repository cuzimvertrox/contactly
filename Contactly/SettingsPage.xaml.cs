using Contactly.Controller;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Contactly
{
    public sealed partial class SettingsPage : Page
    {
        private readonly ConfigController configController = new ConfigController();
        private bool isNotificationVisible = false;

        public SettingsPage()
        {
            this.InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        // Diese Methode wird aufgerufen, wenn die Seite geladen wird
        public async void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Hier wird der Wert für ContactFilePathTextBlock gesetzt, wenn die Seite geladen ist.
            string contactFilePath = await configController.GetSelectedPathAsync();
            ContactFilePathTextBlock.Text = contactFilePath;
        }

        // Diese Methode wird aufgerufen, wenn der "Durchsuchen"-Button geklickt wird
        public async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Erstellen Sie einen Dateiauswahldialog
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");

            // Zeigen Sie den Dateiauswahldialog an
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                // Wenn ein Ordner ausgewählt wurde, setzen Sie den ausgewählten Pfad in die Konfiguration und im TextBlock
                string selectedPath = folder.Path;
                ContactFilePathTextBlock.Text = selectedPath;
                await configController.SaveSelectedPathAsync(selectedPath);

                // Benachrichtigung anzeigen
                ShowNotificationBanner();
            }
        }

        // Diese Methode zeigt den Benachrichtigungsbanner an
        private async void ShowNotificationBanner()
        {
            // Animation für das Einblenden des Banners
            var fadeInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTarget(fadeInAnimation, NotificationBanner);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);

            NotificationBanner.Visibility = Visibility.Visible;
            storyboard.Begin();

            // Hier können Sie den Text für die Benachrichtigung festlegen, falls erforderlich
            // NotificationTextBlock.Text = "Ihre Benachrichtigungstext hier";

            // Warten Sie eine Weile, bevor Sie den Banner ausblenden
            await Task.Delay(TimeSpan.FromSeconds(3));

            HideNotificationBanner();
        }


        // Diese Methode blendet den Benachrichtigungsbanner aus
        private async void HideNotificationBanner()
        {
            // Animation für das Ausblenden des Banners
            var fadeOutAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTarget(fadeOutAnimation, NotificationBanner);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);

            storyboard.Completed += (sender, e) =>
            {
                NotificationBanner.Visibility = Visibility.Collapsed;
            };

            storyboard.Begin();
        }

        private void ShowNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            ShowNotificationBanner();
        }




        // Diese Methode wird aufgerufen, wenn auf die Schaltfläche "Zurück" geklickt wird
        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            // Ihr Code, um zur Hauptseite zu navigieren
            Frame.Navigate(typeof(MainPage));
        }
    }
}
