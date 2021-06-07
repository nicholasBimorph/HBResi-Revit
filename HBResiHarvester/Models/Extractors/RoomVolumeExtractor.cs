using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace HBResiHarvester.Extractors
{
    /// <summary>
    /// Extracts the shells of all the <see cref="Room"/>'s
    /// in the current Revit document.
    /// </summary>
    internal class RoomShellExtractor : IGeometryExtractor<Solid>
    {
        private readonly IEnumerable<Room> _rooms;

        /// <summary>
        /// Construct a <see cref="RoomVolumeExtractor"/>.
        /// </summary>
        internal RoomShellExtractor(IEnumerable<Room> rooms) => _rooms = rooms;

        public IEnumerable<Solid> Extract()
        {
            var solids = new List<Solid>();

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

                        solids.Add(solid);

                    }
                }
            }
        }
       
    }
}
