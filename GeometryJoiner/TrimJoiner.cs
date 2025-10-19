using BaseClasses;
using HostMgd.EditorInput;
using Teigha.DatabaseServices;

namespace GeometryJoiner
{
    public class TrimJoiner : BaseUtility
    {
        public void TrimAndJoin()
        {
            PromptEntityOptions promptEntity = new PromptEntityOptions("Выберите примитив: ");

            PromptEntityResult promptEntityResult = nanoDocumentEditor.GetEntity(promptEntity);

            try
            {
                object curve = GetInputEntity(promptEntityResult);

                nanoDocumentEditor.WriteMessage($"{CheckInputEntity(curve, typeof(Curve))}");
            }
            finally
            {

            }
        }
    }
}
