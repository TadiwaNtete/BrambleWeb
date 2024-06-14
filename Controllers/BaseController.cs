using Bramble.Models.GenericModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BrambleWeb.Controllers
{
	public abstract class BaseController : Controller
	{
		public BaseController()
		{
			if (UserData == null)
				UserData = new BrambleUser() { DisplayName = "Guest", IsArtist = false, UserName = "Guest", IsAdmin = false };
			else
				UserData = UserData;

			if (this.HttpContext != null)
				UserData.GetType().GetProperties().ToList()
				   .ForEach(x => HttpContext.Items.Add(x.Name, x.GetValue(UserData)));
		}
		public class ViewBagFilter : IActionFilter
		{
			public void OnActionExecuting(ActionExecutingContext context) => ViewDataAssignment(context.Controller as Controller);

			public void OnActionExecuted(ActionExecutedContext context) => ViewDataAssignment(context.Controller as Controller);

			private void ViewDataAssignment(Controller controller)
			{
				if (!controller.ViewData.Keys.Any())
					UserData.GetType().GetProperties().ToList().ForEach(x => controller.ViewData
					.Add(x.Name, x.GetValue(UserData)));
			}
		}

		public static BrambleUser UserData { get; set; }
	}
}