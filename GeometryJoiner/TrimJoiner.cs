using BaseClasses;
using HostMgd.EditorInput;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
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
                for (int i = 0; i < selectedObjects.Count - 1; i++)
                {
                    for(int j = i + 1; j < selectedObjects.Count; j++)
                    {
                        ObjectId curveId = selectedObjects[i].ObjectId;
                        ObjectId curve2Id = selectedObjects[j].ObjectId;

                        Curve? curve = transaction.GetObject(curveId, OpenMode.ForRead) as Curve;
                        Curve? curve2 = transaction.GetObject(curve2Id, OpenMode.ForRead) as Curve;


                        curve?.IntersectWith(curve2, Intersect.OnBothOperands, points, IntPtr.Zero, IntPtr.Zero);
                    }
                }
                transaction.Commit();
            }

            foreach(Point3d p in points)
            {
                nanoDocumentEditor.WriteMessage($"{p.X} {p.Y} {p.Z}");
            }
        }
    }
}
