using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Contactly.Controller
{
    public class ConfigController
    {
        // Die Datenklasse für die Konfiguration
        public class AppConfig
        {
            public string VcfFolderPath { get; set; }
        }

        private const string ConfigFileName = "config.json";

        // Der vollständige Pfad zur Konfigurationsdatei
        private string ConfigFilePath => Path.Combine(GetSettingsFolderPath(), ConfigFileName);

        // Erstellt die Konfigurationsdatei, wenn sie nicht existiert
        public void CreateConfigFileIfNotExists()
        {
            try
            {
                string appDataFolderPath = ApplicationData.Current.LocalFolder.Path;
                string settingsFolderPath = Path.Combine(appDataFolderPath, "Settings");
                string documentsFolderPath = Path.Combine("Ex.: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Contactly\\Kontakte");


                // Überprüfen, ob der "Settings"-Ordner vorhanden ist, andernfalls erstellen
                if (!Directory.Exists(settingsFolderPath))
                {
                    Directory.CreateDirectory(settingsFolderPath);
                }

                // Überprüfen, ob die Konfigurationsdatei vorhanden ist, andernfalls erstellen
                if (!File.Exists(ConfigFilePath))
                {
                    var defaultConfig = new AppConfig { VcfFolderPath = documentsFolderPath };
                    string defaultConfigJson = JsonConvert.SerializeObject(defaultConfig);
                    File.WriteAllText(ConfigFilePath, defaultConfigJson);
                }

                // Überprüfen, ob der "Contactly"-Stanmdardordner vorhanden ist, andernfalls erstellen
            }
            catch (Exception ex)
            {
                // Behandeln Sie Ausnahmen, die während des Erstellungsprozesses auftreten können
                Console.WriteLine($"Fehler beim Erstellen der Konfigurationsdatei: {ex.Message}");
            }


        }

        // Überprüft, ob die Konfigurationsdatei vorhanden ist
        public bool ConfigFileExists()
        {
            return File.Exists(ConfigFilePath);
        }

        // Löscht die Konfigurationsdatei
        public void DeleteConfigFile()
        {
            if (File.Exists(ConfigFilePath))
            {
                File.Delete(ConfigFilePath);
            }
        }

        // Lädt die Konfigurationsdaten aus der Datei oder gibt einen leeren Wert zurück, wenn die Datei nicht gefunden wurde
        private async Task<AppConfig> LoadConfigAsync()
        {
            try
            {
                using (StreamReader sr = new StreamReader(ConfigFilePath))
                {
                    string json = await sr.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<AppConfig>(json);
                }
            }
            catch (FileNotFoundException)
            {
                return new AppConfig { VcfFolderPath = string.Empty };
            }
        }

        // Speichert die Konfigurationsdaten in der Datei
        private async Task SaveConfigAsync(AppConfig configData)
        {
            string json = JsonConvert.SerializeObject(configData);

            using (StreamWriter sw = new StreamWriter(ConfigFilePath))
            {
                await sw.WriteAsync(json);
            }
        }

        // Gibt den ausgewählten Pfad aus der Konfigurationsdatei zurück
        public async Task<string> GetSelectedPathAsync()
        {
            var configData = await LoadConfigAsync();
            return configData?.VcfFolderPath;
        }

        // Speichert den ausgewählten Pfad in der Konfigurationsdatei
        public async Task SaveSelectedPathAsync(string selectedPath)
        {
            var configData = await LoadConfigAsync();
            configData.VcfFolderPath = selectedPath;
            await SaveConfigAsync(configData);
        }

        // Hilfsmethode zur Erstellung des Ordnerpfads für die Einstellungen
        private string GetSettingsFolderPath()
        {
            string appDataFolderPath = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(appDataFolderPath, "Settings");
        }
    }
}
