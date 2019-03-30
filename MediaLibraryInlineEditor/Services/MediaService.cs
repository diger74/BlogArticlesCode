using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CMS.EventLog;
using CMS.Helpers;
using CMS.IO;
using CMS.MediaLibrary;
using diger74.Models.Media;
using diger74.Repositories;

namespace diger74.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly string[] _imageExtensions = ".bmp;.gif;.jpg;.jpeg;.png;.svg".Split(';');
        private static readonly string _libraryName = "Default";

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public static ImageModel GetImageModelByURl(string url)
        {
            var match = Regex.Match(url, @"[0-9a-fA-F]{8}[-]([0-9a-fA-F]{4}[-]){3}[0-9A-Fa-f]{12}");
            if (match.Success)
            {
                return GetImageModel(Guid.Parse(match.Value));
            }

            return null;
        }

        public static ImageModel GetImageModel(Guid id)
        {
            try
            {
                var mediaFile = MediaFileInfoProvider.GetMediaFiles()
                    .WhereEquals("FileGUID", id)
                    .TopN(1)
                    .FirstOrDefault();
                var folder = mediaFile?.FilePath.Replace(mediaFile.FileName + mediaFile.FileExtension, string.Empty);
                if (!string.IsNullOrEmpty(folder))
                {
                    folder = folder.Substring(0,folder.Length-1);
                }
                var folderPath = _libraryName;
                if (!string.IsNullOrEmpty(folder))
                {
                    folderPath = $"{_libraryName}\\{folder.Replace("/","\\")}";
                }

                return new ImageModel
                {
                    Id = id,
                    Title = mediaFile?.FileTitle,
                    Url = URLHelper.ResolveUrl(MediaFileURLProvider.GetMediaFileUrl(mediaFile?.FileGUID ?? Guid.Empty, mediaFile?.FileName)),
                    FileName = $"{mediaFile?.FileName}{mediaFile?.FileExtension}",
                    FolderPath = folderPath,
                    FileExtension = mediaFile?.FileExtension.Replace(".", string.Empty),
                    UploadDate = mediaFile?.FileCreatedWhen
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("MediaService", "EXCEPTION", ex);
                return null;
            }
        }

        public IEnumerable<ImageModel> GetAllImages()
        {
            var mediaFiles = _mediaRepository.GetAllMediaFilesByLibraryName()
                .Where(x => _imageExtensions.Contains(x.FileExtension, StringComparer.OrdinalIgnoreCase))
                .ToList();
            var libraries = mediaFiles.Select(x => x.FileLibraryID).Distinct()
                .Select(MediaLibraryInfoProvider.GetMediaLibraryInfo);
            return mediaFiles.Select(
                x =>
                {
                    var library = libraries.FirstOrDefault(l => l.LibraryID == x.FileLibraryID);
                    return new ImageModel
                    {
                        FullFileName = $"{library?.LibraryFolder}\\{x.FileName}{x.FileExtension}",
                        Id = x.FileGUID
                    };
                }).OrderBy(x => x.FullFileName);
        }

        public MediaFolderTree GetMediaFolderTree()
        {
            var tree = new MediaFolderTree
            {
                Name = _libraryName,
                Path = _libraryName,
                Items = new List<MediaFolderTreeItem>()
            };

            var allMediaPaths = _mediaRepository.GetAllMediaFilesByLibraryName()
                .Where(x => _imageExtensions.Contains(x.FileExtension, StringComparer.OrdinalIgnoreCase))
                .Where(x => x.FilePath.Contains("/"))
                .Select(x => x.FilePath.Substring(0, x.FilePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase)))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var mediaPath in allMediaPaths)
            {
                var folders = mediaPath.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).ToList();

                tree.Items = CheckAndAddFolder(tree.Items, tree.Path, folders);
            }

            return tree;
        }

        private IList<MediaFolderTreeItem> CheckAndAddFolder(IList<MediaFolderTreeItem> currentSubtree, string currentRoot, IList<string> pathToAdd)
        {
            if (pathToAdd == null || !pathToAdd.Any()) return currentSubtree;
            var firstDir = pathToAdd.First();

            var subdir = currentSubtree?.FirstOrDefault(x => x.Name.Equals(firstDir));
            if (subdir == null)
            {
                subdir = new MediaFolderTreeItem
                {
                    Name = firstDir,
                    Type = MediaItemType.folder,
                    Path = Path.Combine(currentRoot, firstDir)
                };

                if (currentSubtree == null) currentSubtree = new List<MediaFolderTreeItem>();
                currentSubtree.Add(subdir);
            }

            subdir.Items = CheckAndAddFolder(subdir.Items, subdir.Path, pathToAdd.Skip(1).ToList());

            return currentSubtree;
        }
        
        public MediaFilesSet GetMediaFiles(string path)
        {
            var result = new MediaFilesSet();

            var validPath = path.Replace(_libraryName, string.Empty);
            if (validPath.StartsWith("\\"))
                validPath = validPath.Substring(1);
            validPath = validPath.Replace("\\", "/");
            result.Items = _mediaRepository.GetAllMediaFilesByLibraryName().Where(x =>
                   x.FilePath ==
                   $"{(string.IsNullOrEmpty(validPath) ? string.Empty : validPath + "/")}{x.FileName}{x.FileExtension}")
                .Select(x => new MediaFile()
                {
                    Id = x.FileGUID.ToString(),
                    Link = URLHelper.GetAbsoluteUrl(MediaFileURLProvider.GetMediaFileUrl(x.FileGUID, x.FileName)),
                    Name = $"{x.FileName}{x.FileExtension}",
                    Type = (_imageExtensions.Contains(x.FileExtension) ? MediaItemType.image : MediaItemType.unknown)
                }).ToArray();
            return result;
        }
    }
}