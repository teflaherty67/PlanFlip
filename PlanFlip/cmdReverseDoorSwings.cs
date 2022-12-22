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

            FilteredElementCollector colDoors = new FilteredElementCollector(doc);
            colDoors.OfCategory(BuiltInCategory.OST_Doors);
            colDoors.WhereElementIsNotElementType();

            List<FamilyInstance> leftSwing = new List<FamilyInstance>();

            List<FamilyInstance> rightSwing = new List<FamilyInstance>();

            // Parameter leftParam == null;

            // Parameter rightParam = null;

            // loop through all the parameters in colDoors & find Swing Left and Swing Right

            foreach (FamilyInstance curDoor in colDoors)
            {
                foreach (Parameter curParam in curDoor.Parameters)
                {
                    if (curParam.Definition.Name == "Swing Left")
                    {
                        // do something
                            // set LeftParam equal to curParam??
                            // check the value of the parameter
                            // if it's equal to 0, ignore it
                            // if it's equal to 1, add curDoor to the leftSwing list
                    }

                    else if (curParam.Definition.Name == "Swing Right")
                    {                               
                        // do something else
                            // set rightParam equal to curParam"
                            // check the value of the parameter
                            // if it's equal to 0, ignore it
                            // if it's equal to 1, add curDoor to the rightSwing list
                    }
                }
            }

            // start the transaction
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Reverse Door Swings");

                foreach (FamilyInstance curDoor in leftSwing)
                {
                    // set Swing Left value to 0
                        // leftParam.Set(0)

                    // set Swing Right value to 1
                        // rightParam.Set(1)
                }

                foreach (FamilyInstance curDoor in rightSwing)
                {
                    // set Swing Left value to 1
                        // leftParam.Set(1)

                    // set Swing Right value to 0
                        // rightParam.Set(0)
                }

                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
