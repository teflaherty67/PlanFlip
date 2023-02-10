using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PlanFlip
{
    internal static class Utils
    {
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

        internal static string GetParameterValueByName(Element element, string paramName)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null && paramList.Count > 0)
            {
                Parameter param = paramList[0];
                string paramValue = param.AsValueString();
                return paramValue;
            }

            return "";
        }

        internal static void SetParameterByName(Element element, string paramName, string value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null)
            {
                Parameter param = paramList[0];

                param.Set(value);
            }
        }

       internal static string GetStringBetweenCharacters(string input, string charFrom, string charTo)
        {
            int posFrom = input.IndexOf(charFrom);
            if (posFrom != -1) //if found char
            {
                int posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1) //if found char
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }       
    }
    public static List<ViewSheet> GetSheetsByNumber(Document curDoc, string sheetNumber)
    {
        List<ViewSheet> returnSheets = new List<ViewSheet>();

        //get all sheets
        List<ViewSheet> curSheets = GetAllSheets(curDoc);

        //loop through sheets and check sheet name
        foreach (ViewSheet curSheet in curSheets)
        {
            if (curSheet.SheetNumber.Contains(sheetNumber))
            {
                returnSheets.Add(curSheet);
            }
        }

        return null;
    }

    public static List<ViewSheet> GetAllSheets(Document curDoc)
    {
        //get all sheets
        FilteredElementCollector m_colViews = new FilteredElementCollector(curDoc);
        m_colViews.OfCategory(BuiltInCategory.OST_Sheets);

        List<ViewSheet> m_sheets = new List<ViewSheet>();
        foreach (ViewSheet x in m_colViews.ToElements())
        {
            m_sheets.Add(x);
        }

        return m_sheets;
    }
}
