using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ensure DataDirectory is set before any DB code runs.
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory; // bin\Debug\...
            var dataFolder = Path.Combine(exeFolder, "Data");
            Directory.CreateDirectory(dataFolder); // safe if it exists

            AppDomain.CurrentDomain.SetData("DataDirectory", dataFolder);
        }
    }
}
