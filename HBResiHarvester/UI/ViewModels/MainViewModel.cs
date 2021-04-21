using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Bimorph.WebApi.Core;
using HBResiHarvester.Annotations;

namespace HBResiHarvester.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _generatedId ="";
        private int _totalUploadCount = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public UploadDataCommand UploadDataCommand { get; }

        public int TotalUploadCount
        {
            get => _totalUploadCount;

            set
            {
                _totalUploadCount = value;

                this.OnPropertyChanged(nameof(this.TotalUploadCount));
            }

        }

        public string GeneratedId
        {
            get => _generatedId;

            set
            {
                _generatedId = value;

                this.OnPropertyChanged(nameof(this.GeneratedId));
            }
        }

        public MainViewModel(WebClientService webClientService, JsonSerialization jsonSerialization, DataNodeCollection dataNodeCollection)
        {
            
            this.UploadDataCommand = new UploadDataCommand(this,webClientService, jsonSerialization, dataNodeCollection);



        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
