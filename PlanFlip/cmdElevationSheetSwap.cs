#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace PlanFlip
{
    [Transaction(TransactionMode.Manual)]
    public class cmdElevationSheetSwap : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;            

            // get all viewports
            FilteredElementCollector vpCollector = new FilteredElementCollector(doc);
            vpCollector.OfCategory(BuiltInCategory.OST_Viewports);

            // filter viewports for sheet names that contains "Exterior Elevations"
            
            List<Viewport> eSheets= new List<Viewport>();
            List<Viewport> lrSheets = new List<Viewport>();
            List<string> sheetNum = new List<string>();

            foreach (Viewport vPort in vpCollector)
            {
                string sName = Utils.GetParameterValueByName(vPort, "SheetId");

                if (sName.Contains ("Exterior Elevations"))
                    eSheets.Add(sName);

                foreach(Viewport curVP in eSheets)
                {
                    string vName = Utils.GetParameterValueByName(vPort, "ViewId");

                    if (vName.Contains("Left") || vName.Contains("Right"))
                        lrSheets.Add(vName.Name);
                }

                foreach (Viewport curSheet in lrSheets)
                {
                    string sNumber = Utils.GetParameterValueByName(vPort, "Sheet Number");
                    
                }



            }

            return Result.Succeeded;
        }
    }
}
