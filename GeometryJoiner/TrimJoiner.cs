using BaseClasses;
using HostMgd.EditorInput;
using System.Drawing;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Runtime;
using Utilities.Collections.Lists;

namespace GeometryJoiner
{
    public class TrimJoiner : BaseUtility
    {

        public void TrimAndJoin()
        {
            Assembly? teigha = Assembly.GetAssembly(typeof(Curve));
            IEnumerable<Type> curveTypes = teigha?.ExportedTypes.Where(t => t.BaseType == typeof(Curve));

            List<TypedValue> typedValues = new List<TypedValue>() { new TypedValue((int)DxfCode.Operator, "<or") };
            foreach (Type type in curveTypes)
            {
                RXClass rxClass = RXClass.GetClass(type);
                typedValues.Add(new TypedValue((int)DxfCode.Start, rxClass.DxfName));
            }
            typedValues.Add(new TypedValue((int)DxfCode.Operator, "or>"));
            SelectionFilter filters = new SelectionFilter(typedValues.ToArray());


            PromptSelectionResult selectionResult = nanoDocumentEditor.GetSelection(filters);

            SelectedObjects selectedObjects = new SelectedObjects(selectionResult);

            Point3dCollection points = new Point3dCollection();

            using (Transaction transaction = nanoDatabase.TransactionManager.StartTransaction())
            {
                foreach (SelectedObject s1 in selectedObjects)
                {
                    foreach (SelectedObject s2 in selectedObjects)
                    {
                        if (s1 != s2)
                        {
                            ObjectId curveId = s1.ObjectId;
                            ObjectId curve2Id = s2.ObjectId;

                            Curve? curve = transaction.GetObject(curveId, OpenMode.ForRead) as Curve;
                            Curve? curve2 = transaction.GetObject(curve2Id, OpenMode.ForRead) as Curve;

                            curve?.IntersectWith(curve2, Intersect.OnBothOperands, points, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                }

                foreach(SelectedObject selectedObject in selectedObjects)
                {
                    Curve? curve = transaction.GetObject(selectedObject.ObjectId, OpenMode.ForWrite) as Curve;

                    DBObjectCollection slicedCurves =  curve!.GetSplitCurves(points);

                    BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(nanoDatabase.CurrentSpaceId, OpenMode.ForWrite);

                    foreach(DBObject slicedCurve in slicedCurves)
                    {
                        Entity entity = (Entity)slicedCurve;

                        blockTableRecord.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }
                    curve.Erase(true);
                }
                transaction.Commit();
            }
        }
    }
}
