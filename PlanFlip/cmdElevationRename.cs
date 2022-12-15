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

            // get all the elevation views

            List<View> viewList = GetAllElevationViews(doc);
           
            Element curTitleOnSheet = viewList.FirstElement();

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

            // loop through the view collector
            foreach (View curView in viewList)
            {
                // change view name
                if (curView.Name.Contains ("Left"))
                    curView.Name.Replace ("Left", "Right");
                else if (curView.Name.Contains ("Right"))       
                    curView.Name.Replace ("Right", "Left");

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

        public static List<View> GetAllElevationViews(Document doc)
        {
            List<View> returnList = new List<View>();

            FilteredElementCollector colViews = new FilteredElementCollector(doc);
            colViews.OfClass(typeof(View));

            // loop through views and check for elevation views
            foreach (View x in colViews)
            {
                if (x.GetType() == typeof(ViewSection))
                {
                    if (x.IsTemplate == false)
                    {
                        if (x.ViewType == ViewType.Elevation)
                        {
                            // add view to list
                            returnList.Add(x);
                        }
                    }
                }
            }

            return returnList;
        }
    }
}
