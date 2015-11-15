using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using FileDirBrowse.DAL;
using FileDirBrowse.Models;

namespace FileDirBrowse.Controllers
{
    public class BrowseController : ApiController

    {   
        public IEnumerable<FileViewModel> Get() => ApplicationFileManager.GetFiles(null);

        public IEnumerable<FileViewModel> Post(PathModel path) => ApplicationFileManager.GetFiles(path.Path);

        public IEnumerable<FileViewModel> UpStairs(PathModel path)
        {
            var filePath = ApplicationFileManager.Decode(path.Path);
            IList<FileViewModel> files = new List<FileViewModel>();
            if (filePath == "root") files = ApplicationFileManager.GetRootDirectories();
            else if (Directory.Exists(filePath))
            {
                var di = new DirectoryInfo(filePath);
                files = di.Parent != null
                    ? ApplicationFileManager.GetFiles(di.Parent.FullName)
                    : ApplicationFileManager.GetRootDirectories();
            }
            return files;
        }
    }
}