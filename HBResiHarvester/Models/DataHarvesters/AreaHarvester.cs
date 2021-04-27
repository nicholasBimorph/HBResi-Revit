using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using Bimorph.WebApi.Core;
using Bimorph.WebApi.Core.Types;
using HBResiHarvester.Extensions;
using HBResiHarvester.Interfaces;
using HBResiHarvester.Settings;
using Parameter = Bimorph.WebApi.Core.Parameter;

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

        private readonly IList<string> _areaParameterNames;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="areas"></param>
        /// <param name="serializer"></param>
        internal AreaHarvester(
            IEnumerable<Area> areas , 
            ISerializer serializer, 
            ApplicationServices applicationServices)
        {
            _areas = areas;

            _serializer = serializer;

            _areaParameterNames = applicationServices.ParameterNames;
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
                var parameters = new List<Parameter>();

                foreach (string parameterName in _areaParameterNames)
                {
                    if (parameterName == "Area")
                    {
                       string parameterValue = areaObject.LookupParameter(parameterName).AsValueString();

                       var parameter = new Parameter(parameterName, parameterValue);

                       parameters.Add(parameter);
                    }

                    else
                    {
                        string parameterValue = areaObject.LookupParameter(parameterName).GetParameterValueAsString();

                        var parameter = new Parameter(parameterName, parameterValue);

                        parameters.Add(parameter);
                    }

                    

                }

                var bimorphArea = new BimorphArea(parameters);

                string jObject = _serializer.Serialize<BimorphArea>(bimorphArea);

                var node = new DataNode(jObject, typeof(BimorphArea));

                dataNodes.Add(node);

                //string level = areaObject.LookupParameter(ApplicationSettings.LevelParameterName).GetParameterValueAsString();

                //var levelParam = new Parameter(ApplicationSettings.LevelParameterName, level);
                //parameters.Add(levelParam);

                //string plot = areaObject.LookupParameter(ApplicationSettings.PlotParameterName).GetParameterValueAsString();

                //var plotParam = new Parameter(ApplicationSettings.PlotParameterName, plot);
                //parameters.Add(plotParam);

                //string block = areaObject.LookupParameter(ApplicationSettings.BlockParameterName).GetParameterValueAsString();

                //var blockParam = new Parameter(ApplicationSettings.BlockParameterName, block);
                //parameters.Add(blockParam);

                //string spaceType = areaObject.LookupParameter(ApplicationSettings.SpaceTypeParameterName).GetParameterValueAsString();

                //var spaceTypeParam = new Parameter(ApplicationSettings.SpaceTypeParameterName, spaceType);
                //parameters.Add(spaceTypeParam);

                //string unitType = areaObject.LookupParameter(ApplicationSettings.UnitTypeParameterName).GetParameterValueAsString();

                //var unitTypeParam = new Parameter(ApplicationSettings.UnitTypeParameterName, unitType);
                //parameters.Add(unitTypeParam);

                //string tenure = areaObject.LookupParameter(ApplicationSettings.TenureParameterName).GetParameterValueAsString();

                //var tenureParam = new Parameter(ApplicationSettings.TenureParameterName, tenure);
                //parameters.Add(tenureParam);

                //string accesibilityType = areaObject.LookupParameter(ApplicationSettings.AccessibilityTypeParameterName).GetParameterValueAsString();

                //var accesibilityTypeParam = new Parameter(ApplicationSettings.AccessibilityTypeParameterName, accesibilityType);
                //parameters.Add(accesibilityTypeParam);

                //string area = areaObject.LookupParameter(ApplicationSettings.AreaParameterName).AsValueString();

                //var areaParam = new Parameter(ApplicationSettings.AreaParameterName, area);
                //parameters.Add(areaParam);

                //string number = areaObject.LookupParameter(ApplicationSettings.NumberParameterName).GetParameterValueAsString();

                //var numberParam = new Parameter(ApplicationSettings.NumberParameterName, number);
                //parameters.Add(numberParam);

                //string areaType = areaObject.LookupParameter(ApplicationSettings.AreaTypeParameterName).GetParameterValueAsString();

                //var areaTypeParam = new Parameter(ApplicationSettings.AreaTypeParameterName, areaType);
                //parameters.Add(areaTypeParam);

                ////var bimorphArea = new BimorphArea(plot, level, block, spaceType, unitType, tenure, accesibilityType, area, number, areaType);

                //var bimorphArea = new BimorphArea(parameters);

                //string jObject = _serializer.Serialize<BimorphArea>(bimorphArea);

                //var node = new DataNode(jObject, typeof(BimorphArea));

                //dataNodes.Add(node);

            }

            return dataNodes;
        }
    }
}
