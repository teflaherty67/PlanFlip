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
            
            List<Viewport> eSheets = new List<Viewport>();
            List<ViewSheet> lrSheets = new List<ViewSheet>();
            List<string> sheetNum = new List<string>();

            foreach (Viewport vPort in vpCollector)
            {
                ViewSheet curSheet = doc.GetElement(vPort.SheetId) as ViewSheet;
                View curView = doc.GetElement(vPort.ViewId) as View;

                string sName = Utils.GetParameterValueByName(curSheet, "Sheet Name");
                string vName = Utils.GetParameterValueByName(curView, "Name");

                if (sName.Contains("Exterior Elevations"))
                {
                    if (vName.Contains("Left") || vName.Contains("Right"))
                        lrSheets.Add(curSheet);
                }
            }

            ElevationSwapForm1 curForm = new ElevationSwapForm1(lrSheets);
            curForm.ShowDialog();

            if(curForm.DialogResult==System.Windows.Forms.DialogResult.OK)
            {

                // create variable for current sheet numbers

                string curLeftNum1 = curForm.GetComboBox1Item();
                string curRightNum1 = curForm.GetComboBox2Item();
                string curLeftNum2 = curForm.GetComboBox3Item();
                string curRightNum2 = curForm.GetComboBox4Item();

                // extract the number portion of the sheet number              

                string numLeft1 = Utils.GetStringBetweenCharacters(curLeftNum1, "A", "last character");

                string numRight1 = Utils.GetStringBetweenCharacters(curRightNum1, "A", "last character");

                string numLeft2 = Utils.GetStringBetweenCharacters(curLeftNum2, "A", "last character");

                string numRight2 = Utils.GetStringBetweenCharacters(curRightNum2, "A", "last character");


                // start the transaction

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Renumber Elevation Sheets");

                    




                    t.Commit();
                } 
            }
                
            return Result.Succeeded;
        }
    }
}
