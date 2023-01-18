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

                int sNumLeft1 = curLeftNum1.Length;
                int sNumRight1 = curRightNum1.Length;
                int sNumLeft2 = curLeftNum2.Length;
                int sNumRight2 = curRightNum2.Length;
                

                // current sheet with Left Elevation

                if(sNumLeft1 == 3)
                {
                   char numLeft1 = curLeftNum1[1];
                }
                
                if(sNumLeft1 == 4)
                {
                   string numLeft1 = curLeftNum1[1, 2];
                }

                // current sheet with Right Elevation

                if (sNumRight1 == 3)
                {
                    char numRight1 = curRightNum1[1];
                }

                if (curRightNum1.length = 4)
                {
                    numRight1 = curRightNum1[1, 2];
                }

                // split Left Elevation sheet 2

                if (curLeftNum2.length = 3)
                {
                    numLeft2 = curLeftNum2[1, 1];
                }

                if (curLeftNum2.length = 4)
                {
                    numLeft2 = curLeftNum2[1, 2];
                }

                // split Right Elevation sheet 2

                if (curRightNum2.length = 3)
                {
                    numRight2 = curRightNum2[1, 1];
                }

                if (curRightNum2.length = 4)
                {
                    numRight2 = curRightNum2[1, 2];
                }

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
