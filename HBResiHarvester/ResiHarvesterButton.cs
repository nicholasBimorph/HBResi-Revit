﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimorph.WebApi.Core;
using Bimorph.WebApi.Core.Types;
using HBResiHarvester.Extensions;
using HBResiHarvester.Settings;
using HBResiHarvester.UI.ViewModels;
using Area = Autodesk.Revit.DB.Area;

namespace HBResiHarvester
{
    [Transaction(TransactionMode.Manual)]
    public class ResiHarvesterButton : IExternalCommand
    {

        /// <summary>
        /// The tool tip text displayed to the user.
        /// </summary>
        public string ToolTip { get; }

        /// <summary>
        /// The name of the button in the Revit ribbon.
        /// </summary>
        public string VisibleButtonName { get; }

        /// <summary>
        /// The internal button name visible to the developer.
        /// </summary>
        public string InternalButtonName { get; }

        public ResiHarvesterButton()
        {
            this.VisibleButtonName = "Harvester";
            this.InternalButtonName = "cmd_Harvester";
            this.ToolTip = "A tooltip";
        }


        /// <summary>Overload this method to implement and external command within Revit.</summary>
        /// <returns> The result indicates if the execution fails, succeeds, or was canceled by user. If it does not
        /// succeed, Revit will undo any changes made by the external command. </returns>
        /// <param name="commandData"> An ExternalCommandData object which contains reference to Application and View
        /// needed by external command.</param>
        /// <param name="message"> Error message can be returned by external command. This will be displayed only if the command status
        /// was "Failed".  There is a limit of 1023 characters for this message; strings longer than this will be truncated.</param>
        /// <param name="elements"> Element set indicating problem elements to display in the failure dialog.  This will be used
        /// only if the command status was "Failed".</param>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           var document = commandData.Application.ActiveUIDocument.Document;

           var jsonSerializer = new Bimorph.WebApi.Core.JsonSerialization();

           var webClientService = new WebClientService(jsonSerializer);

           var areaObjects = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_Areas).WhereElementIsNotElementType()
                .Cast<Area>();


            var nodeCollection = new DataNodeCollection();

            foreach (var areaObject in areaObjects)
            {
                string level = areaObject.LookupParameter(ApplicationSettings.LevelParameterName).AsValueString();


                string block = areaObject.LookupParameter(ApplicationSettings.BlockParameterName)
                    .GetParameterValueAsString();
               string spaceType = areaObject.LookupParameter(ApplicationSettings.SpaceTypeParameterName).GetParameterValueAsString();
                string unitType = areaObject.LookupParameter(ApplicationSettings.UnitTypeParameterName).GetParameterValueAsString();
                string tenure = areaObject.LookupParameter(ApplicationSettings.TenureParameterName).GetParameterValueAsString();
                string accesibilityType = areaObject.LookupParameter(ApplicationSettings.AccessibilityTypeParameterName).GetParameterValueAsString();
                string area = areaObject.LookupParameter(ApplicationSettings.AreaParameterName).AsValueString();
                string number = areaObject.LookupParameter(ApplicationSettings.NumberParameterName).GetParameterValueAsString();
                string areaType = areaObject.LookupParameter(ApplicationSettings.AreaTypeParameterName).GetParameterValueAsString();

                var bimorphArea = new BimorphArea("",level,block,spaceType,unitType,tenure,accesibilityType,area,number,areaType);

               string jObject = jsonSerializer.Serialize<BimorphArea>(bimorphArea);

                var node = new DataNode(jObject, typeof(BimorphArea));

               nodeCollection.Nodes.Add(node);
            }

            var mainViewModel = new MainViewModel(webClientService, jsonSerializer, nodeCollection);

            var mainWindow = new MainWindow(mainViewModel);

            mainWindow.ShowDialog();


            return Result.Succeeded;
        }
    }
}
