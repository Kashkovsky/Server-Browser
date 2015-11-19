using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileDirBrowse.Models;

namespace FileDirBrowse.DAL
{
    public class ApplicationFileManager
    {
        public static IList<FileViewModel> GetRootDirectories()

        {
            var browseList = new List<FileViewModel>();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives.Where(drive => drive.IsReady))
            {
                switch (drive.DriveType)
                {
                    case DriveType.CDRom:
                        browseList.Add(new FileViewModel(drive.RootDirectory)
                        {
                            ApplicationCategory = new ApplicationCategory(FileType.DiscRoot)
                        });
                        break;

                    case DriveType.Fixed:
                        browseList.Add(new FileViewModel(drive.RootDirectory)
                        {
                            ApplicationCategory = new ApplicationCategory(FileType.FixedRoot)
                        });
                        break;

                    case DriveType.Network:
                        browseList.Add(new FileViewModel(drive.RootDirectory)
                        {
                            ApplicationCategory = new ApplicationCategory(FileType.NetworkRoot)
                        });
                        break;

                    case DriveType.Removable:
                        browseList.Add(new FileViewModel(drive.RootDirectory)
                        {
                            ApplicationCategory = new ApplicationCategory(FileType.RemovableRoot)
                        });
                        break;

                    case DriveType.Unknown:
                        break;
                    case DriveType.NoRootDirectory:
                        break;
                    case DriveType.Ram:
                        break;
                    default:

                        browseList.Add(new FileViewModel(drive.RootDirectory));
                        break;
                }
            }

            return browseList;
        }
        public static IList<FileViewModel> GetFiles(string path)
        {
            var result = new List<FileViewModel>();
            if (string.IsNullOrEmpty(path)) return GetRootDirectories();
            path = Decode(path);
            try
            {
                var dirs = Directory.GetDirectories(path, "*.*",
                    SearchOption.TopDirectoryOnly);

                result.AddRange(dirs.Select(dir => new DirectoryInfo(dir))
                    .Select(drive => new FileViewModel(drive) { ApplicationCategory = new ApplicationCategory(GetFileType(drive.ToString()))}));
                string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                result.AddRange(files.Select(file => new FileInfo(file))
                    .Select(file => new FileViewModel(file) {ApplicationCategory = new ApplicationCategory(GetFileType(file.ToString()))}));
                result.Sort((a, b) =>
                {
                    var name1 = a.Name;
                    var name2 = b.Name;
                    if (a.ApplicationCategory.Value == FileType.Folder) name1 = " " + name1;
                    if (b.ApplicationCategory.Value == FileType.Folder) name2 = " " + name2;
                    return string.Compare(name1, name2, StringComparison.Ordinal);
                });
                return result;
            }
            catch (Exception)
            {
                
            }
            return result;
        }

        public static string Encode(string path)
        {
            return path.Replace("\\", "/");
        }

        public static string Decode(string path)
        {
            return path.Replace("/", "\\");
        }

        private static FileType GetFileType(string path)
        {
            switch (Path.GetExtension(path))
            {
                case "":
                    return FileType.Folder;
                case ".jpg":
                case ".png": case ".gif":
                    return FileType.Picture;
                case ".dll": 
                    return FileType.Dll;
                case ".config":
                    return FileType.Config;
                case ".zip":
                    return FileType.Zip;
                case ".mp3": case ".vaw": case ".m4a":
                    return FileType.Music;
                case ".mov": case ".mpeg": case ".mp4":
                    return  FileType.Video;
                case ".exe":
                    return FileType.Exe;
                default:
                    return FileType.File;
            }
        }
    }

}
    
