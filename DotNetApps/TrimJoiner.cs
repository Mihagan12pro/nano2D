using DotNetApps;
using DotNetUtilities.Collections.Lists;
using HostMgd.EditorInput;
using System.Reflection;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Runtime;

namespace GeometryJoiner
{
    public class TrimJoiner : BaseDotNetApp
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

                foreach(Point3d point in points)
                {
                    nanoDocumentEditor.WriteMessage($"{point.X} {point.Y} {point.Z}");
                }

                List<Curve> curves = new List<Curve>();
                List<DBObjectCollection> dbCollection = new List<DBObjectCollection>();
                foreach(SelectedObject selectedObject in selectedObjects)
                {
                    Curve? curve = transaction.GetObject(selectedObject.ObjectId, OpenMode.ForWrite) as Curve;

                    dbCollection.Add(curve!.GetSplitCurves(points));

                    //BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(nanoDatabase.CurrentSpaceId, OpenMode.ForWrite);

                    curve.Erase(true);

                    //foreach (DBObject slicedCurve in slicedCurves)
                    //{
                    //    Entity entity = (Entity)slicedCurve;

                    //    blockTableRecord.AppendEntity(entity);
                    //    transaction.AddNewlyCreatedDBObject(entity, true);

                    //    curves.Add((Curve)entity);
                    //}
                }

                BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(nanoDatabase.CurrentSpaceId, OpenMode.ForWrite);
                foreach (DBObjectCollection objects in dbCollection)
                {
                    foreach(DBObject obj in objects)
                    {
                        Entity entity = (Entity)obj;

                        blockTableRecord.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }
                }

                transaction.Commit();
            }

            filters = new SelectionFilter(typedValues.ToArray());


            selectionResult = nanoDocumentEditor.GetSelection(filters);

            selectedObjects = new SelectedObjects(selectionResult);
        }
    }
}
