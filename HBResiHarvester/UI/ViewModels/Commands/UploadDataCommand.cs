using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bimorph.WebApi.Core;

namespace HBResiHarvester.UI.ViewModels
{
    public class UploadDataCommand : ICommand
    {
        private readonly WebClientService _webClientService;

        private readonly JsonSerialization _jsonSerialization;

        DataNodeCollection  _dataNodeCollection;

        private MainViewModel _mainViewModel;

        string urlPost = "https://localhost:44360/DataNodes";

        internal  UploadDataCommand(
            MainViewModel mainViewModel,
            WebClientService webClientService, 
            JsonSerialization jsonSerialization, 
            DataNodeCollection dataNodeCollection)
        {
            _webClientService = webClientService;

            _jsonSerialization = jsonSerialization;

            _dataNodeCollection = dataNodeCollection;

            _mainViewModel = mainViewModel;
        }
        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        /// <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            string response = _webClientService.PostRequest(urlPost, _dataNodeCollection);

            _mainViewModel.GeneratedId = response;
        }

        /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
        public event EventHandler CanExecuteChanged;

    }
}
