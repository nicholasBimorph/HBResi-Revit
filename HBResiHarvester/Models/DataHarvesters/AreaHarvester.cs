using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimorph.WebApi.Core;
using Bimorph.WebApi.Core.Types;
using HBResiHarvester.Extensions;
using HBResiHarvester.Interfaces;
using HBResiHarvester.Settings;

namespace HBResiHarvester.DataHarvesters
{
    /// <summary>
    /// This class is responsible to harvest area objects
    /// from the active Revit document.
    /// </summary>
    public class AreaHarvester : IHarvester
    {
        private readonly IEnumerable<Area> _areas;

        private readonly ISerializer _serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areas"></param>
        /// <param name="serializer"></param>
        internal AreaHarvester(IEnumerable<Area> areas , ISerializer serializer)
        {
            _areas = areas;

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

            foreach (var areaObject in _areas)
            {
                string level = areaObject.LookupParameter(ApplicationSettings.LevelParameterName).GetParameterValueAsString();

                string plot = areaObject.LookupParameter(ApplicationSettings.PlotParameterName).GetParameterValueAsString();

                string block = areaObject.LookupParameter(ApplicationSettings.BlockParameterName).GetParameterValueAsString();

                string spaceType = areaObject.LookupParameter(ApplicationSettings.SpaceTypeParameterName).GetParameterValueAsString();

                string unitType = areaObject.LookupParameter(ApplicationSettings.UnitTypeParameterName).GetParameterValueAsString();

                string tenure = areaObject.LookupParameter(ApplicationSettings.TenureParameterName).GetParameterValueAsString();

                string accesibilityType = areaObject.LookupParameter(ApplicationSettings.AccessibilityTypeParameterName).GetParameterValueAsString();

                string area = areaObject.LookupParameter(ApplicationSettings.AreaParameterName).AsValueString();

                string number = areaObject.LookupParameter(ApplicationSettings.NumberParameterName).GetParameterValueAsString();

                string areaType = areaObject.LookupParameter(ApplicationSettings.AreaTypeParameterName).GetParameterValueAsString();

                var bimorphArea = new BimorphArea(plot, level, block, spaceType, unitType, tenure, accesibilityType, area, number, areaType);

                string jObject = _serializer.Serialize<BimorphArea>(bimorphArea);

                var node = new DataNode(jObject, typeof(BimorphArea));

                dataNodes.Add(node);

            }

            return dataNodes;
        }
    }
}
