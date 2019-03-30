using System.Text;
using System.Web.Mvc;
using diger74.Extensions;
using diger74.Services;
using Newtonsoft.Json;

namespace diger74.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IHashService _hashService;

        public MediaController(IMediaService mediaService, IHashService hashService)
        {
            _mediaService = mediaService;
            _hashService = hashService;
        }
 
        [HttpGet]
        public JsonResult GetAllImages()
        {
            return Json(_mediaService.GetAllImages(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMediaFolderTree(string hash)
        {
            var mediaTree = _mediaService.GetMediaFolderTree();
            var newHash = _hashService.GetHashString(JsonConvert.SerializeObject(mediaTree));
            if (newHash == hash)
            {
                mediaTree = null;
            }
            else
            {
                mediaTree.Hash = newHash;
            }
            return Json(mediaTree, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMediaFiles(string path, string hash)
        {
            var mediaSet = _mediaService.GetMediaFiles(path);
            var newHash = _hashService.GetHashString(JsonConvert.SerializeObject(mediaSet));
            if (newHash == hash)
            {
                mediaSet = null;
            }
            else
            {
                mediaSet.Hash = newHash;
            }
            return Json(mediaSet, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}