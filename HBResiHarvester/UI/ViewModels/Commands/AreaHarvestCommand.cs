using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HBResiHarvester.Interfaces;

namespace HBResiHarvester.UI.ViewModels
{
    public class AreaHarvestCommand : ICommand
    {
        private IHarvester _dataHarvester;
        internal AreaHarvestCommand(IHarvester dataHarvester)
        {
            _dataHarvester = dataHarvester;
        }

        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        /// <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.
        /// If the command does not require data to be passed, this object can be set to <see langword="null" />.
        /// </param>
        public void Execute(object parameter)
        {
            var viewModel = (MainViewModel) parameter;

            var dataNodes =  _dataHarvester.Harvest();

          foreach (var dataNode in dataNodes)
              viewModel.DataNodes.Add(dataNode);

        }

        /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
        public event EventHandler CanExecuteChanged;
    }
}
