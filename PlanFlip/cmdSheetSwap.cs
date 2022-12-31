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
    public class cmdSheetSwap : IExternalCommand
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

            /* 
             filter the viewports to find those with SheetId.Contains "Exterior Elevations" or with
            Sheet Name = Exterior Elevations

            filter that list to find Exterior Elevation sheets that have a ViewId or View Name that
            contains Left or Right

            from that list write the following data to Excel, to an Array??

            Sheet Number & View Name

                if sheetNumber for Left Elevation != sheetNumber for Right Elevation
                    {
                        change the integer part of the sheetNumber of Right Elevation to $ &
                        change the integer part of the sheetNumber of Left Elevation to the
                                current number of Right Elevation
                        then change the integer part of the sheetNumber of Right Eelvation
                                from $ to the prevoius number.
                    }
                Else if sheetNumber for Left Elevation == sheetNumber for Right Elevation
                    {
                        do nothing
                    }            
             */
          
            

            return Result.Succeeded;
        }
    }
}
