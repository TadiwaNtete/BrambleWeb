using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

#region Bramble Usings
using Bramble.Models.GenericModels;
using Bramble.Service.Interface;
using Bramble.Framework.EnumeratedType;
using System.Security.Principal;
using System.Web.Http.ModelBinding;
using BrambleWeb.Data;
using System.Security.Claims;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http;

#endregion
namespace BrambleWeb.Controllers
{

	public class AccountController : BaseController
	{
		private readonly ICommonService _commonService;
		private readonly IAccountService _accountService;
		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;

		public AccountController(ICommonService commonService, IAccountService accountService, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, BrambleDBContext dbContext)
		{
			_commonService = commonService;
			_accountService = accountService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login() => View();

		[HttpGet]
		public IActionResult LogInWithCredentials(UserModel uiUserModel)
		{
			var userValidation = _accountService.ValidateUser(uiUserModel);
			if (userValidation.Status != StatusConstants.Success)
				return Json(new { success = false, message = userValidation.Message });

			#region Return Assignment
			UserData.UserName = uiUserModel.UserName;
			UserData.DisplayName = string.IsNullOrEmpty(uiUserModel.DISPLAY_NAME) ? uiUserModel.UserName : uiUserModel.DISPLAY_NAME;
			UserData.Email = uiUserModel.Email;

			#endregion

			return RedirectToAction("Index", "BrambleHome");
		}

		[HttpPost]
		public async Task<IActionResult> UserRegistration(UserModel model)
		{

			#region Initial Validations
			if (!_commonService.IsValid<UserModel>(model, new List<string>() { "USERNAME", "EMAIL", "PASSWORDHASH" }))
				return Json(new { message = $@"{MessageConstants.BadInput} user model", success = false });

			model.CREATED_DATE = DateTime.Now;
			var password = model.PasswordHash;

			var userValidation = _accountService.ValidateUserName(model);
			if (userValidation.Status == StatusConstants.Success)
				return Json(new { message = "An account with that username already exists, please try again", success = false });

			#endregion

			#region Identity Claim Declaration

			var newUserClaim = new ClaimModel() //creation of a Claim based on user's name
			{
				CREATED_DATE = model.CREATED_DATE,
				UserId = model.Id,
				Id = 1,
				ClaimType = ClaimTypes.Name,
				ClaimValue = model.UserName
			};

			#endregion

			var userCreation = await _userManager.CreateAsync(model, password); //user creation method
			if (!userCreation.Succeeded)
				return Json(new { status = false, message = $@"{MessageConstants.Exception} {userCreation.Errors.FirstOrDefault().Description}" });

			var userClaimAdditions = await _userManager.AddClaimAsync(model, newUserClaim.ToClaim()); //claims for the user need to be added AFTER creation
			if (!userClaimAdditions.Succeeded)
				return Json(new { success = false, Message = $@"{MessageConstants.Exception} {userClaimAdditions.Errors.FirstOrDefault().Description}" });

			var returnMessageString = string.IsNullOrEmpty(model.DISPLAY_NAME) ?
				model.UserName : model.DISPLAY_NAME;

			return Json(new { success = false, Status = StatusConstants.Success, Message = $@"Successfully registered new user {returnMessageString} have fun!" });

		}
	}
}
