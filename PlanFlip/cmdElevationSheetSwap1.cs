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
    public class cmdElevationSheetSwap1 : IExternalCommand
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

            // part 1 - get all the sheets

            // use a FEC to get all the sheets? or all the viewports

            FilteredElementCollector vpCollector = new FilteredElementCollector(doc);
            vpCollector.OfCategory(BuiltInCategory.OST_Viewports);

            // part 2 - filter the sheets down to left & right elevation sheets

            List<Viewport> eSheets = new List<Viewport>();
            List<ViewSheet> sheetsLeft = new List<ViewSheet>();
            List<ViewSheet> sheetsRight = new List<ViewSheet>();

            // filter the collector to get sheets with name "Exterior Elevations"
            // and view names that contain "Left" or "Right"

            foreach (Viewport vPort in vpCollector)
            {
                ViewSheet curSheet = doc.GetElement(vPort.SheetId) as ViewSheet;
                View curView = doc.GetElement(vPort.ViewId) as View;

                string sName = Utils.GetParameterValueByName(curSheet, "Sheet Name");
                string vName = Utils.GetParameterValueByName(curView, "Name");

                if (sName.Contains("Exterior Elevations"))
                {
                    if (vName.Contains("Left"))
                        sheetsLeft.Add(curSheet);

                    if (vName.Contains("Right"))
                        sheetsRight.Add(curSheet);
                }
            }

            // filter the new list to get sheets with view names containing "Left" and "Right"

            // part 3 - get the numerical portion of the sheet numbers in the new list

            // part 4 - swap the numerical portion of the sheet numbers

            // sheets with right elevations change numerial to $

            // sheets with left elevations change numerial to current right elevation numerial

            // sheets with right elevations change $ to current left elevation numerial



            return Result.Succeeded;
        }
    }
}