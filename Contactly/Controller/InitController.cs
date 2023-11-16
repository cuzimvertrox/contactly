using System;
using System.Threading.Tasks;
using Contactly.Controller;

namespace Contactly.Controller
{
    /// <summary>
    /// Ein Controller, der die Anwendungsinitialisierung durchführt.
    /// </summary>
    public class InitController
    {
        private readonly ConfigController _configController;

        /// <summary>
        /// Konstruktor für den InitController.
        /// </summary>
        public InitController()
        {
            // Initialisieren Sie den ConfigController.
            _configController = new ConfigController();
        }

        /// <summary>
        /// Diese Methode wird beim Start der Anwendung aufgerufen, um die Initialisierung durchzuführen.
        /// </summary>
        /// <returns>Ein Task, der die Initialisierung durchführt.</returns>
        public async Task InitializeAsync()
        {
            // Überprüfen Sie, ob die Config-Datei existiert, und erstellen Sie sie, wenn sie nicht vorhanden ist.
            _configController.CreateConfigFileIfNotExists();
        }
    }
}
