using System.Collections.Generic;
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
        /// </summary>
        /// <param name="areas"></param>
        /// <param name="serializer"></param>
        internal AreaHarvester(
            IEnumerable<Area> areas,
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
                    if (parameterName == AreaParameterNames.AreaParameterName)
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

                //var bimorphArea = new BimorphArea(parameters);

                var bimorphArea = new BimorphArea(parameters);
                

                string jObject = _serializer.Serialize(bimorphArea);

                var node = new DataNode(jObject, typeof(BimorphArea));

                dataNodes.Add(node);
            }

            return dataNodes;
        }
    }
}