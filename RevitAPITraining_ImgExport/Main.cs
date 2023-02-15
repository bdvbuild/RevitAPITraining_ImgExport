using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Transaction = Autodesk.Revit.DB.Transaction;

namespace RevitAPITraining_ImgExport
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ViewPlan viewPlan = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .Cast<ViewPlan>()
                .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan &&
                                    v.Name.Equals("Level 1"));

            ImageExportOptions imgOption = new ImageExportOptions()
            {
                FilePath = path,
            };
            imgOption.SetViewsAndSheets(new List<ElementId> { viewPlan.Id });

            using (var ts = new Transaction(doc, "img export"))
            {
                ts.Start();
                doc.ExportImage(imgOption);
                ts.Commit();
            }

            return Result.Succeeded;
        }
    }
}
