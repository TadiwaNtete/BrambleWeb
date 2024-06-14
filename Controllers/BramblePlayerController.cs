using Microsoft.AspNetCore.Mvc;

#region Bramble Usings
using Bramble.Models.DataModels.ClassModels;
using Bramble.Models.DataModels.DBModels;
using Bramble.Framework.EnumeratedType;
using Bramble.Service.Interface;
using Bramble.Service.Service;
using Microsoft.AspNetCore;

#endregion

namespace BrambleWeb.Controllers
{
	public class BramblePlayerController : BaseController
	{
		private readonly ICommonService _commonService;
		private readonly IPlayerService _playerService;
		private readonly IFileService _fileService;
		public BramblePlayerController(ICommonService commonService, IPlayerService playerService, IFileService fileService)
		{
			_commonService = commonService;
			_playerService = playerService;
			_fileService = fileService;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SongPlayer(string identifierKey)
		{
			var searchResponseModel = _playerService.GetPlayerSong(identifierKey);
			if (searchResponseModel.Status != StatusConstants.Success)
				return View(new SingleEntryResponseModel() { Status = StatusConstants.Error, Message = searchResponseModel.Message });

			return View(searchResponseModel);
		}

		public IActionResult RenderSongList()
		{
			var songListDBResponse = _fileService.ParsedSingleEntryCall();
			if (songListDBResponse.Status != StatusConstants.Success)
				return PartialView(new SingleEntryResponseModel() { });
			else
				return PartialView(PartialViews.SongList, songListDBResponse);
		}

		[HttpPost]
		public IActionResult SubmitComment(string comment)
		{
			var submissionResponse = _playerService.SubmitComment(comment);

			return submissionResponse.Status == StatusConstants.Success ?
				Json(new { Success = true }) : Json(new { Success = false });
		}
	}
}
