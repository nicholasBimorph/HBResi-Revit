using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Bimorph.WebApi.Core;
using Bimorph.WebApi.Core.Models.Types;
using HBResiHarvester.Extensions;
using HBResiHarvester.Interfaces;

namespace HBResiHarvester.DataHarvesters
{
    /// <summary>
    /// This class harvests all available <see cref="Room"/> volumes
    /// in the current Revit document.
    /// </summary>
    internal class RoomVolumeHarvester:IHarvester
    {
        private readonly IEnumerable<Room> _rooms;

        private readonly ISerializer _serializer;

        /// <summary>
        /// Construct a <see cref="RoomVolumeHarvester"/>.
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="serializer"></param>
        internal RoomVolumeHarvester(IEnumerable<Room> rooms, ISerializer serializer)
        {
            _rooms = rooms;

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

            foreach (var room in _rooms)
            {
                var shell = room.ClosedShell;

                foreach (var geoObject in shell)
                {
                    var geoObjectInstance = geoObject as GeometryInstance;

                    if (null == geoObjectInstance) continue;

                    foreach (var instObj in geoObjectInstance.SymbolGeometry)
                    {
                        var solid = instObj as Solid;
                        if (null == solid || 
                            0 == solid.Faces.Size || 
                            0 == solid.Edges.Size) continue;


                      var bimorphMesh =  solid.ToBimorphMesh(geoObjectInstance);

                      string jObject = _serializer.Serialize(bimorphMesh);

                      var node = new DataNode(jObject, typeof(BimorphMesh));

                      dataNodes.Add(node);

                    }
                }
            }
            return dataNodes;
        }

    }
}
