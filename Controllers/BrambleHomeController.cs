using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions;

#region Bramble Usings
using Bramble.Models.GenericModels;
using Bramble.Service.Interface;
using Bramble.Framework.EnumeratedType;
using Bramble.Models.DataModels.ClassModels;
using Bramble.Models.DataModels.DBModels;
using Microsoft.AspNetCore.Authentication;

#endregion


namespace BrambleWeb.Controllers
{
	public class BrambleHomeController : BaseController
	{
		#region Dependencies and Constructor
		private readonly ICommonService _commonService;
		private readonly IYoutubeApiService _youtubeApi;
		private readonly IConfiguration _configuration;
		private readonly IFileService _fileService;

		public BrambleHomeController(ICommonService commonService, IYoutubeApiService youtubeApi, IConfiguration configuration, IFileService fileService)
		{
			_commonService = commonService;
			_youtubeApi = youtubeApi;
			_configuration = configuration;
			_fileService = fileService;
		}

		#endregion

		#region Generic Launch Methods
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorResponseModel { Message = "ERROR OCCURRED.", REQUEST_ID = "1", Status = "ERROR" });//return View(new a { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

		#endregion

		#region Partial Views
		public async Task<IActionResult> RenderBrambleHomeSelection()
		{
			try
			{
				var responseHomeSelection = _fileService.ParsedSingleEntryCall();
				if (responseHomeSelection.Status != StatusConstants.Success)
					return PartialView(PartialViews.HomeSelection, responseHomeSelection);
				//additional code will need to be written to check if a temporary local file exists
				return PartialView(PartialViews.HomeSelection, responseHomeSelection);
			}
			catch (Exception ex)
			{
				return Error();
				throw;
			}
		}

		#endregion
	}
}