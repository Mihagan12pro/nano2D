using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.Geometry;

namespace Utilities.Collections.Sets
{
    public class PointsSet : HashSet<Point3d>
    {
        public void AddPoint3DCollection(Point3dCollection collection)
        {
            foreach (Point3d p in collection)
            {
                Add(p);
            }
        }
    }
}
