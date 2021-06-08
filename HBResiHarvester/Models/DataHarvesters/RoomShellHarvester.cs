using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimorph.WebApi.Core;
using Bimorph.WebApi.Core.Models.Types;
using HBResiHarvester.Extensions;
using HBResiHarvester.Interfaces;

namespace HBResiHarvester.DataHarvesters
{
    internal class RoomShellHarvester : IHarvester
    {
        private readonly IEnumerable<Solid> _roomShells;

        private readonly ISerializer _serializer;
        internal RoomShellHarvester(IEnumerable<Solid> roomShells, ISerializer serializer)
        {
            _roomShells = roomShells;

            _serializer = serializer;
        }
        /// <summary>
        /// Harvests a particular set of data from a
        /// Revit document.
        /// </summary>
        /// <returns></returns>
        public IList<DataNode> Harvest()
        {
            var dataNodes = new List<DataNode>();

            foreach (var roomShell in _roomShells)
            {
               var bimorphMesh = roomShell.ToBimorphMesh();

               string jObject = _serializer.Serialize(bimorphMesh);

               var node = new DataNode(jObject, typeof(BimorphMesh));

               dataNodes.Add(node);
            }

            return dataNodes;
        }
    }
}
