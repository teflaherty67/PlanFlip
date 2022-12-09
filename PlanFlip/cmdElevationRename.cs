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
    public class cmdElevationRename : IExternalCommand
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

            // get all the views
            FilteredElementCollector colViews = new FilteredElementCollector(doc);
            colViews.OfCategory(BuiltInCategory.OST_Views);

            // set a variable for the view name
            Element curViewName = colViews.FirstElement();

            string viewName = curViewName.Name;

            // find the Title on Sheet Parameter & assign it to a variable
            Element curTitleOnSheet = colViews.FirstElement();

            string curTitle = "";
            foreach (Parameter curParam in curTitleOnSheet.Parameters)
            {
                if(curParam.Definition.Name == "Title on Sheet")
                {
                    curTitle = curParam.ToString();
                }
            }

            // start the transaction
            Transaction t = new Transaction(doc);
            t.Start("Rename Elevations");

            // I think I need a loop here
            foreach (View curView in colViews)
            {
                // change view name
                if (curViewName.Contains ("Left"))
                    curViewName.Replace ("Left", "Right");
                else if (curViewName.Contains ("Right"))       
                    curViewName.Replace ("Right", "Left");

                // change the title on sheet
                if (curTitle.Contains("Left"))
                    curTitle.Replace("Left", "Right");
                else if (curTitle.Contains("Right"))
                    curTitle.Replace("Right", "Left");
            }                     

            // commit the changes
            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
    }
}
