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
        private readonly BimorphAPIClientService _webClientService;

        internal  UploadDataCommand(BimorphAPIClientService webClientService)
        {
            _webClientService = webClientService;
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
            var viewModel = (MainViewModel) parameter;

            var dataNodeCollection = new DataNodeCollection(viewModel.UniqueStreamId);

            foreach (var dataNode in viewModel.DataNodes)
                dataNodeCollection.Nodes.Add(dataNode);

            string response = "";

            if (viewModel.AutoSync)
            {
                string requestUrl = ApiEndPoints.PutNodeCollectionEndPoint + viewModel.UniqueStreamId;

                response = _webClientService.PutRequest(requestUrl, dataNodeCollection);

                return;
            }

            response = _webClientService.PostRequest(ApiEndPoints.PostNodeCollectionEndPoint, dataNodeCollection);

            viewModel.TotalUploadCount = dataNodeCollection.Nodes.Count;
        }

        /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
        public event EventHandler CanExecuteChanged;

    }
}
