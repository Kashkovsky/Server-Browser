using System.IO;
using FileDirBrowse.DAL;

namespace FileDirBrowse.Models
{
    public enum FileType
    {
        Folder,
        File,
        Zip,
        Exe,
        Music,
        Video,
        Dll,
        Picture,
        Config,
        FixedRoot,
        NetworkRoot,
        RemovableRoot,
        DiscRoot,
        SysRoot
    }

    public class ApplicationCategory
    {
        public ApplicationCategory(FileType type)
        {
            Value = type;
        }

        public FileType Value { get; }
    }

    public class FileViewModel
    {
        public FileViewModel(FileInfo info)
        {
            Name = info.Name;
            Location = info.DirectoryName;
            Path = ApplicationFileManager.Encode(info.FullName);
            Length = info.Length;
        }

        public FileViewModel(DirectoryInfo info)
        {
            Name = info.Name;
            Location = info.Parent != null ? info.Parent.FullName : "";
            Path = info.FullName;
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }
        public string CurrentParentPath { get; set; }
        public ApplicationCategory ApplicationCategory { get; set; }

        public Size Size { get { return Length <= 10485760
                    ? Size.TenOrLess
                    : Length > 10485760 && Length <= 52428800 ? Size.From10To50 : Size.Over100;
            }
        }
                

        private long Length { get; }
    }

    public enum Size
    {
        TenOrLess,
        From10To50,
        Over100
    }

    public class PathModel
    {
        public string Path { get; set; }
    }
}