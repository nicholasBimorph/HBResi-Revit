using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBResiHarvester.Interfaces
{
    /// <summary>
    /// A contract which all concrete implementations should implement
    /// in order to extract geometrical data from
    /// an  entity that contains geometrical attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGeometryExtractor<out T>
    {
        /// <summary>
        /// Extracts a collection of a defined geometrical information
        /// from an entity that contains geometrical attributes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Extract();
    }
}
