using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bimorph.WebApi.Core;

namespace HBResiHarvester.Interfaces
{
    /// <summary>
    /// A contract which all data <see cref="IHarvester"/>'s
    /// have to implement.
    /// </summary>
    public interface IHarvester
    {
        /// <summary>
        /// Harvests a particular set of data from a
        /// Revit document.
        /// </summary>
        /// <returns></returns>
        IList<DataNode> Harvest();
    }
}
