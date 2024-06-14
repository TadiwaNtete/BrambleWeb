using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;
using System.IO;

#region Bramble Usings
using Bramble.Models;
using Bramble.Models.GenericModels;

using Bramble.Service;
using Bramble.Service.Interface;
using Bramble.Service.Service;
using Bramble.Framework.EnumeratedType;
using Bramble.Models.DataModels;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Bramble.Models.DataModels.ClassModels;
using Bramble.Models.DataModels.DBModels;
#endregion

namespace BrambleWeb.Controllers
{
	public class BrambleSongController : BaseController
	{
		private readonly IWebHostEnvironment _environment;
		private readonly IFileService _fileService;
		private readonly ICommonService _commonService;
		public BrambleSongController(IWebHostEnvironment environment, IFileService fileService, ICommonService commonService)
		{
			_environment = environment;
			_fileService = fileService;
			_commonService = commonService;
		}
		public IActionResult Index()
		{
			return View(new BrambleSingleEntry() { Status = StatusConstants.NotAvailable });
		}

		[HttpPost]
		[DisableRequestSizeLimit,
		RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
		ValueLengthLimit = int.MaxValue)]
		public async Task<IActionResult> UploadSongDetails()
		{
			try
			{
				#region Preliminary Validation
				//Checking the validitity of the Form of the HTTPContext, if null, do not proceed. we only need 
				if (HttpContext.Request.Form == null || HttpContext.Request.Form.Files.Count <= 0)
					return Json(new { success = false, message = $@"{MessageConstants.BadInput} invalid request" });

				#endregion

				#region Form Deserialization
				//this DeserializeForm method allows us to assign the data from the form to a new object 
				var singleEntry = _commonService.DeserializeForm<BrambleSingleEntry>(HttpContext.Request.Form);

				if (singleEntry == null)
					return Json(new { success = false, message = $@"{MessageConstants.BadProcess} preparing data" });

				if (!_commonService.IsValid(singleEntry, new List<string>() { "SONG_NAME", "SONG_AUTHOR" }))
					return Json(new { success = false, message = $@"{MessageConstants.BadProcess} preparing data" });

				#endregion

				#region Create Directory
				//we need to first create the directory
				var createDirectoryResponse = _fileService.FolderCreation(FileLocations.UploadLocation, singleEntry.SONG_NAME + singleEntry.SONG_AUTHOR); 
				if (createDirectoryResponse.Status != StatusConstants.Success)
					return Json(new { success = false, message = $@"{MessageConstants.CreateFail} Folder Directory" });

				singleEntry.DIRECTORY_PATH = createDirectoryResponse.Message;

				#endregion

				singleEntry.SONG_FILE = new BrambleFile(HttpContext.Request.Form.Files.Where(x => x.ContentType.ToUpper().Contains("AUDIO")).FirstOrDefault()) { DIRECTORY_PATH = singleEntry.DIRECTORY_PATH };
				singleEntry.THUMBNAIL = new BrambleFile(HttpContext.Request.Form.Files.Where(x => x.ContentType.ToUpper().Contains("IMAGE")).FirstOrDefault()) { DIRECTORY_PATH = singleEntry.DIRECTORY_PATH };

				#region Upload Song to Directory
				//upload the song file to the directory
				var fileUploadResponse = singleEntry.SONG_FILE != null ?
					await _fileService.UploadBrambleFile(singleEntry.SONG_FILE) : new BrambleFileResposnseModel() { Status = StatusConstants.Error, Message = $@"{MessageConstants.BadInput}: audio file was invalid" };

				if (fileUploadResponse.Status == StatusConstants.Error)
					return Json(new { success = false, message = fileUploadResponse.Message });

				singleEntry.SONG_FILE = fileUploadResponse.SingleDataResponse;
				singleEntry.SONG_PATH = fileUploadResponse.SingleDataResponse.DIRECTORY_PATH + "\\" + fileUploadResponse.SingleDataResponse.BRAMBLE_FILE.FileName;

				#endregion

				#region Upload Image to Directory
				//upload the image file to the directory
				fileUploadResponse = singleEntry.THUMBNAIL != null ?
					await _fileService.UploadBrambleFile(singleEntry.THUMBNAIL) : new BrambleFileResposnseModel() { Status = StatusConstants.NotAvailable };

				if (fileUploadResponse.Status == StatusConstants.Error)
					return Json(new { success = false, message = fileUploadResponse.Message });

				singleEntry.THUMBNAIL = fileUploadResponse.SingleDataResponse;
				singleEntry.IMAGE_PATH = fileUploadResponse.SingleDataResponse.DIRECTORY_PATH + "\\" + fileUploadResponse.SingleDataResponse.BRAMBLE_FILE.FileName;

				#endregion

				#region Database Save
				//save the single entry to the database along with its image and song file
				var saveSongDBResponse = _commonService.SaveDBSong(singleEntry);

				if (saveSongDBResponse.Status != StatusConstants.Success)
					return Json(new { success = false, message = saveSongDBResponse.Message });

				#endregion

				return Json(new { success = true, message = saveSongDBResponse.Message });
			}
			catch (Exception ex)
			{
				#region Exception

				return Json(new { success = false, message = ex.Message });

				#endregion
			}
		}
	}
}
