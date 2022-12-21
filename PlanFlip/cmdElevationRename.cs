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

            List<View> viewList = Utils.GetAllElevationViews(doc);

            List<View> viewList2 = new List<View>();
           
            // MK edit: this line isn't needed
            //Element curTitleOnSheet = viewList.FirstElement;

            // start the transaction
            Transaction t = new Transaction(doc);
            t.Start("Rename Elevations");

            // loop through the view collector
            foreach (View curView in viewList)
            {
                // MK edit: added variable for parameter
                Parameter titleParam = null;

                // MK edit: moved this to inside the foreach loop
                string curTitle = "";
                foreach (Parameter curParam in curView.Parameters)
                {
                    if (curParam.Definition.Name == "Title on Sheet")
                    {
                        titleParam = curParam;

                        // MK edit: changed this line to use the AsString() method
                        curTitle = curParam.AsString();
                    }
                }

                // change view name
                if (curView.Name.Contains("Left") == true)
                    // MK edit: added curView.Name to front of line
                    curView.Name = curView.Name.Replace ("Left", "$Right");
                else if (curView.Name.Contains("Right") == true)
                    // MK edit: added curView.Name to front of line
                    curView.Name = curView.Name.Replace ("Right", "$Left");

                viewList2.Add(curView);
               
                // change the title on sheet
                if (curTitle.Contains("Left"))
                    // MK edit: added curView.Name to front of line
                    titleParam.Set(curTitle.Replace("Left", "Right"));
                else if (curTitle.Contains("Right"))
                    // MK edit: added curView.Name to front of line
                    titleParam.Set(curTitle.Replace("Right", "Left"));
            }

            foreach (View curView in viewList2)
            {
                // change view name
                if (curView.Name.Contains("$Right") == true)
                    curView.Name = curView.Name.Replace("$Right", "Right");
                else if (curView.Name.Contains("$Left") == true)
                    curView.Name = curView.Name.Replace("$Left", "Left");
            }

            // commit the changes
            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
    }
}
