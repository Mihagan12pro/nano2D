using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Teigha.DatabaseServices;

namespace BaseClasses
{
    public abstract class BaseUtility : IDisposable
    {
        protected readonly Document nanoDocument;

        protected readonly Editor nanoDocumentEditor;

        protected readonly Database nanoDatabase;

        public bool CheckInputEntity(object promptObject, Type type)
        {
            return (promptObject.GetType().BaseType == type || promptObject.GetType() == type);
        }


        public Entity GetInputEntity(ObjectId id)
        {
            using (Transaction transaction = nanoDatabase.TransactionManager.StartTransaction())
            {

                Entity? entity = transaction.GetObject(id, OpenMode.ForRead) as Entity;

                transaction.Commit();

                return entity!;
            }
        }


        public Entity GetInputEntity(PromptSelectionResult promptResult)
        {
            return GetInputEntity(promptResult);
        }


        public Entity GetInputEntity(SelectedObject selectedObject)
        {
            return GetInputEntity(selectedObject.ObjectId);
        }


        //PromptSelectionResult


        public void Dispose()
        {
            
        }

        public BaseUtility()
        {
            nanoDocument = Application.DocumentManager.CurrentDocument;

            nanoDocumentEditor = nanoDocument.Editor;

            nanoDatabase = nanoDocument.Database;
        }
    }
}
