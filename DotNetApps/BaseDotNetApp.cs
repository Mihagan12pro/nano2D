using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.DatabaseServices;

namespace DotNetApps
{
    public class BaseDotNetApp : IDisposable
    {
        protected readonly Document nanoDocument;

        protected readonly Editor nanoDocumentEditor;

        protected readonly Database nanoDatabase;


        public void Dispose()
        {

        }

        public BaseDotNetApp()
        {
            nanoDocument = Application.DocumentManager.CurrentDocument;

            nanoDocumentEditor = nanoDocument.Editor;

            nanoDatabase = nanoDocument.Database;
        }
    }
}
