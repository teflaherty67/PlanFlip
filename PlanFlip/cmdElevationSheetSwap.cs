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
                string vName = curView.Name;

                if (sName.Contains("Exterior Elevations"))
                {
                    if (vName.Contains("Left") || vName.Contains("Right"))
                        lrSheets.Add(curSheet);
                }
            }

            ElevationSwapForm curForm = new ElevationSwapForm(lrSheets);
            curForm.ShowDialog();

            if(curForm.DialogResult==System.Windows.Forms.DialogResult.OK)
            {

                // create variable for current sheet numbers

                string curLeftNum1 = curForm.GetComboBox1Item();
                string curRightNum1 = curForm.GetComboBox2Item();

                string numLeft1 = Utils.GetStringBetweenCharacters(curLeftNum1, curLeftNum1[0].ToString(),
                    curLeftNum1[curLeftNum1.Length - 1].ToString());

                string numRight1 = Utils.GetStringBetweenCharacters(curRightNum1, curRightNum1[0].ToString(),
                    curRightNum1[curRightNum1.Length - 1].ToString());

                if (curForm.GetCheckBox1() == true)
                {
                    string curLeftNum2 = curForm.GetComboBox3Item();
                    string curRightNum2 = curForm.GetComboBox4Item();

                    string numLeft2 = Utils.GetStringBetweenCharacters(curLeftNum2, curLeftNum2[0].ToString(),
                   curLeftNum2[curLeftNum2.Length - 1].ToString());

                    string numRight2 = Utils.GetStringBetweenCharacters(curRightNum2, curRightNum2[0].ToString(),
                        curRightNum2[curRightNum2.Length - 1].ToString());
                } 

                // start the transaction

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Reorder Elevation Sheets");

                    List<ViewSheet> matchingLeftSheets = Utils.GetSheetsByNumber(doc, numLeft1);
                    List<ViewSheet> matchingRightSheets = Utils.GetSheetsByNumber(doc, numRight1);

                    foreach (ViewSheet sheet in matchingLeftSheets)
                    {
                        sheet.SheetNumber = sheet.SheetNumber.Replace(numLeft1, numRight1 + "zz");
                    }

                    foreach (ViewSheet sheet in matchingRightSheets)
                    {
                        sheet.SheetNumber = sheet.SheetNumber.Replace(numRight1, numLeft1);
                    }

                    foreach (ViewSheet sheet in matchingLeftSheets)
                    {
                        sheet.SheetNumber = sheet.SheetNumber.Replace(numRight1 + "zz", numRight1);
                    }

                    t.Commit();
                } 
            }
                
            return Result.Succeeded;
        }
    }
}