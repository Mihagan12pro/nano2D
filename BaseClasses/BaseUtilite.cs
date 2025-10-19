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

        protected bool CheckInputEntity(object promptObject, Type type)
        {
            return (promptObject.GetType().BaseType == type);
        }


        protected Entity GetInputEntity(PromptEntityResult promptResult)
        {
            if (promptResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = nanoDatabase.TransactionManager.StartTransaction())
                {
                    ObjectId id = promptResult.ObjectId;

                    Entity? entity = transaction.GetObject(id, OpenMode.ForRead) as Entity;

                    transaction.Commit();

                    return entity!;
                }
            }

            throw new NullReferenceException();
        }

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
