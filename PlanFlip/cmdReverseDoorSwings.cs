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

            // get all the doors in the project & create lists by swing

            List<FamilyInstance> leftSwing = Utils.GetAllDoorsBySwing(doc, "Swing Left");

            List<FamilyInstance> rightSwing = Utils.GetAllDoorsBySwing(doc, "Swing Right");

            // start the transaction
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Reverse Door Swings");
            } 

            // loop through each door in each list
           
            // check to see if the swing parameter exists

            // change left to right

            // change right to left

            // commit the changes

            
            
           

            return Result.Succeeded;
        }
    }
}
