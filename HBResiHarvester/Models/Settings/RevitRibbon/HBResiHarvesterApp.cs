using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using HBResiHarvester.UI.ViewModels;

namespace HBResiHarvester.RevitApplication
{
    public class HBResiHarvesterApp: IExternalApplication
    {
        /// <summary>Implement this method to execute some tasks when Autodesk Revit starts.</summary>
        /// <param name="application">A handle to the application being started.</param>
        /// <returns>Indicates if the external application completes its work successfully.</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "HB Resi Harvester";

            application.CreateRibbonTab(tabName);

            RibbonPanel ribbonPanelProductivity = application.CreateRibbonPanel(tabName, "Harvester");

            this.AddButton(ribbonPanelProductivity,new ResiHarvesterButton());


            return Result.Succeeded;
        }

        private  void AddButton(RibbonPanel ribbonPanel, ResiHarvesterButton buttonData)
        {
            PushButtonData pushButtonData = new PushButtonData(buttonData.InternalButtonName,
                buttonData.VisibleButtonName, Assembly.GetExecutingAssembly().Location, buttonData.GetType().FullName);

            PushButton pushButton = (PushButton)ribbonPanel.AddItem(pushButtonData);
            pushButton.ToolTip = buttonData.ToolTip;

        }

        /// <summary>Implement this method to execute some tasks when Autodesk Revit shuts down.</summary>
        /// <param name="application">A handle to the application being shut down.</param>
        /// <returns>Indicates if the external application completes its work successfully.</returns>
        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }
    }
}
