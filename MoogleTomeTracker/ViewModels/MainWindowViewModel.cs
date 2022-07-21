using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoogleTomeTracker.Data;
using MoogleTomeTracker.Library;
using MoogleTomeTracker.Models;

#nullable disable

namespace MoogleTomeTracker.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public Store Data = new();

        [ObservableProperty] private List<MogItem> _mogItems;
        [ObservableProperty] private List<MogItem> _trackedItems;
        [ObservableProperty] private string _title;
        [ObservableProperty] private string _tomeURL;
        [ObservableProperty] private int _totalTomestones;


        public MainWindowViewModel()
        {
            MogItems = Data.MogData.MogItems;
            
            if (string.IsNullOrWhiteSpace(Data.MogData.TomestoneName))
            {
                Title = "Moogle Tomestone Tracker";
            }
            else
            {
                Title = $"Moogle Tomestone Tracker <Irregular tomestones of {Data.MogData.TomestoneName}>";
            }
            TomeURL = Data.MogData.TomeURL;
            var tomes = 0;
            foreach(var item in MogItems)
            {
                if(item.IsTracked && !item.IsAcquired)
                {
                    tomes += item.Cost;
                }
            }
            TotalTomestones = tomes;
        }

        public void SaveData()
        {
            Data.Save();
            MogItems = Data.MogData.MogItems;
            
            if (string.IsNullOrWhiteSpace(Data.MogData.TomestoneName))
            {
                Title = "Moogle Tomestone Tracker";
            }
            else
            {
                Title = $"Moogle Tomestone Tracker <Irregular tomestones of {Data.MogData.TomestoneName}>";
            }
            TomeURL = Data.MogData.TomeURL;
            var tomes = 0;
            foreach (var item in MogItems)
            {
                if (item.IsTracked && !item.IsAcquired)
                {
                    tomes += item.Cost;
                }
            }
            TotalTomestones = tomes;
        }   
    }
}
