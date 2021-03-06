using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Bimorph.WebApi.Core;
using HBResiHarvester.Annotations;
using HBResiHarvester.DataHarvesters;
using HBResiHarvester.Interfaces;

namespace HBResiHarvester.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        
        private bool _canUpload = false;
        private bool _autoSync = false;
        private string _uniqueStreamId;
        private int _totalUploadCount;
        private Document _document;

        /// <summary>
        /// An event which is raised if any property
        /// of this <see cref="MainViewModel"/> changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The command to upload data to the Bimorph server.
        /// </summary>
        public UploadDataCommand UploadDataCommand { get; }

        /// <summary>
        /// A collection of all the <see cref="IDataNode"/>'s
        /// constructed from the Revit document.
        /// </summary>
        public IList<DataNode> DataNodes { get; }

        /// <summary>
        /// Gets the total amount of data that was uploaded
        /// to the Bimorph server.
        /// </summary>
        public int TotalUploadCount
        {
            get => _totalUploadCount;

            set
            {
                _totalUploadCount = value;

                this.OnPropertyChanged(nameof(this.TotalUploadCount));
            }

        }

        /// <summary>
        /// Determines whether data can be uploaded
        /// to the Bimorph server or not.
        /// </summary>
        public bool CanUploadData => this.CanUpload();

        /// <summary>
        /// The unique stream id to send data to the
        /// Bimorph server.
        /// </summary>
        public string UniqueStreamId
        {
            get => _uniqueStreamId;

            set
            {
                _uniqueStreamId = value;

                this.OnPropertyChanged(nameof(this.UniqueStreamId));

                this.OnPropertyChanged(nameof(this.CanUploadData));
            }
        }

        /// <summary>
        /// Determines whether the data that will be uploaded
        /// will modify an existing <see cref="DataNodeCollection"/>
        /// in the data base.
        /// </summary>
        public bool AutoSync
        {
            get => _autoSync;

            set
            {
                _autoSync = value;

                this.OnPropertyChanged(nameof(this.AutoSync));
            }
        }

        /// <summary>
        /// Construct a <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="webClientService"></param>
        /// <param name="areaHarvester">TEMPORARY METHOD will not be here when turned in to command </param>
        internal  MainViewModel(ApplicationServices applicationServices, IHarvester areaHarvester)
        {
            _document = applicationServices.Document;

            _uniqueStreamId = _document.Title;

            this.DataNodes = new List<DataNode>();

            this.UploadDataCommand = new UploadDataCommand( applicationServices.WebClientService);

            //This will be wrapped in a command, when more harvesting options are added to the UI.
            var dataNodes =  areaHarvester.Harvest();

            foreach (var dataNode in dataNodes)
                this.DataNodes.Add(dataNode);
        }

        /// <summary>
        /// Determines whether data can be uploaded
        /// to the Bimorph server or not.
        /// </summary>
        private bool CanUpload()
        {
            return this.UniqueStreamId != _document.Title;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
