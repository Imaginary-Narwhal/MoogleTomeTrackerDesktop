global using Microsoft.Toolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestoreWindowPlace;

namespace MoogleTomeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public WindowPlace WindowPlace { get; set; }

        public static string Storage = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moogle Tome Tracker");

        public App()
        {
            if(!Directory.Exists(Storage))
            {
                Directory.CreateDirectory(Storage);
            }

            this.WindowPlace = new WindowPlace(Path.Combine(Storage, "placement.config"));

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.WindowPlace.Save();
        }
    }
}
