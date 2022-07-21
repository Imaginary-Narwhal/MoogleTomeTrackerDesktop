using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoogleTomeTracker.Models;
using System.IO;
using Newtonsoft.Json;

#nullable disable

namespace MoogleTomeTracker.Data
{
    public partial class Store : ObservableObject
    {
        [ObservableProperty] private MogData _mogData;

        public Store()
        {
            MogData = new();

            if (!File.Exists(Path.Combine(App.Storage, "mog_data.json")))
            {
                File.WriteAllText(
                    Path.Combine(App.Storage, "mog_data.json"),
                    JsonConvert.SerializeObject(MogData)
                );
            }

            MogData = JsonConvert.DeserializeObject<MogData>(
                File.ReadAllText(Path.Combine(App.Storage, "mog_data.json")));
        }

        public void Save()
        {
            File.WriteAllText(
                    Path.Combine(App.Storage, "mog_data.json"),
                    JsonConvert.SerializeObject(MogData)
                );
        }

        public void Reload()
        {
            MogData = JsonConvert.DeserializeObject<MogData>(
                File.ReadAllText(Path.Combine(App.Storage, "mog_data.json")));
        }
    }

    public partial class MogData : ObservableObject
    {
        [ObservableProperty] private List<MogItem> _mogItems;
        [ObservableProperty] private string _tomestoneName;
        [ObservableProperty] private string _tomeURL;
    }


}
