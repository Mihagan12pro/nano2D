using HostMgd.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Collections.Lists
{
    public class SelectedObjects : List<SelectedObject>
    {
        public SelectedObjects(PromptSelectionResult result)
        {
            for (int i = 0; i < result.Value.Count; i++)
            {
                Add(result.Value[i]);
            }
        }
    }
}
