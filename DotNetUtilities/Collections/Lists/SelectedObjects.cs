using HostMgd.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetUtilities.Collections.Lists
{
    public class SelectedObjects : List<SelectedObject>
    {
        public SelectedObjects(PromptSelectionResult promptSelectionResult)
        {
            for (int i = 0; i < promptSelectionResult.Value.Count; i++)
            {
                Add(promptSelectionResult.Value[i]);
            }
        }
    }
}
