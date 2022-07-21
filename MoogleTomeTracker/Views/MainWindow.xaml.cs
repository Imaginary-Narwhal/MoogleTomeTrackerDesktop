using AdonisUI.Controls;
using MoogleTomeTracker.Data;
using MoogleTomeTracker.Models;
using MoogleTomeTracker.Library;
using System.Windows;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using MoogleTomeTracker.ViewModels;
using System.Windows.Controls;

#nullable disable

namespace MoogleTomeTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindowViewModel viewModel = new();
        public MainWindow()
        {
            InitializeComponent();

            ((App)Application.Current).WindowPlace.Register(this);

            this.DataContext = viewModel;
        }

        private void GatherNewMogItems(object sender, RoutedEventArgs e)
        {
            var url = URLBox.Text;
            var result = AdonisUI.Controls.MessageBox.Show("Creating new list will delete all data for both current list and tracked items",
                         "Warning!",
                         AdonisUI.Controls.MessageBoxButton.OKCancel,
                         AdonisUI.Controls.MessageBoxImage.Warning);


            if(result.Equals(AdonisUI.Controls.MessageBoxResult.OK))
            {
                GatherItemsButton.Content = "Gathering ...";
                GatherItemsButton.IsEnabled = false;


                BackgroundWorker worker = new();
                worker.DoWork += delegate
                {

                    try
                    {
                        viewModel.Data.MogData = MogScrape.GetMogItemsFromWeb(url).Result;
                    }
                    catch (Exception)
                    {
                        this.Dispatcher.Invoke(() => {
                            AdonisUI.Controls.MessageBox.Show("There was a problem reading the URL. Please check that you have the correct URL and try again.", "Error");
                        });
                    }
                };

                worker.RunWorkerCompleted += delegate
                {
                    GatherItemsButton.Content = "Gather New Items";
                    GatherItemsButton.IsEnabled = true;

                    viewModel.SaveData();
                };
                worker.RunWorkerAsync();
                
            }
        }

        private void URLTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                GatherItemsButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void MogItemSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItem != null)
            {

                var mogItem = listBox.SelectedItem as MogItem;

                mogItem.IsTracked = !mogItem.IsTracked;

                listBox.SelectedItem = null;

                viewModel.SaveData();
            }
        }

        private void TrackedItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if(listBox.SelectedItem != null)
            {
                var mogItem = listBox.SelectedItem as MogItem;
                mogItem.IsAcquired = !mogItem.IsAcquired;
                listBox.SelectedItem = null;
                viewModel.SaveData();
            }
        }
    }
}
