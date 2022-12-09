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
    public class cmdReverseDoorSwings : IExternalCommand
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

            // get all the doors in the project
            // collect all the doors in the project

            FilteredElementCollector colDoors = new FilteredElementCollector(doc);
            colDoors.OfCategory(BuiltInCategory.OST_Doors);
            colDoors.WhereElementIsNotElementType();
           
            // issue warning if no doors are present
            if (colDoors.GetElementCount() == 0)
            {
                // alert the user
                TaskDialog.Show("Error", "There are no doors in the project.");
            }

            // create varaible for the swing parameter??

            // start the transaction
            Transaction t = new Transaction(doc);
            t.Start("Reverse Door Swings");

            // loop through each door

            // check to see if the swing parameter exists

            // change left to right

            // change right to left

            // commit the changes
            t.Commit();

            // dispose the transaction
            t.Dispose();

            return Result.Succeeded;
        }
    }
}
