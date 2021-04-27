using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bimorph.WebApi.Core;
using Autodesk.Revit.DB;
using HBResiHarvester.Settings;

namespace HBResiHarvester
{
    internal class ApplicationServices
    {
        public Document Document { get; }

        public BimorphAPIClientService WebClientService { get; }

        public IList<string> ParameterNames { get; }

        public ApplicationServices(Document document , BimorphAPIClientService webClientService)
        {
            this.Document = document;

            this.WebClientService = webClientService;

            this.ParameterNames = new List<string>
            {
                AreaParameterNames.LevelParameterName,
                AreaParameterNames.PlotParameterName,
                AreaParameterNames.BlockParameterName,
                AreaParameterNames.SpaceTypeParameterName,
                AreaParameterNames.UnitTypeParameterName,
                AreaParameterNames.TenureParameterName,
                AreaParameterNames.AccessibilityTypeParameterName,
                AreaParameterNames.AreaParameterName,
                AreaParameterNames.NumberParameterName,
                AreaParameterNames.AreaTypeParameterName
            };


        }

    }
}
