using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop;

using OriGames.Facts.Web.Controllers.Facts.Queries;
using OriGames.Facts.Web.Mediatr.Notifications;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

public class SiteController : Controller
{
	private readonly IMediator _mediator;
	private readonly IWebHostEnvironment _environment;

	private readonly List<SelectListItem> _subjects;

	public SiteController(IMediator mediator, IWebHostEnvironment environment, IJSRuntime jsRuntime) 
	{
		_mediator = mediator;
		_environment = environment;

		_subjects = new List<string> {
				"Связь с разработчиком",
				"Жалоба",
				"Предложение",
				"Другое"
			}.Select(x => new SelectListItem { Value = x, Text = x })
			.ToList();
	}

	public IActionResult About()
	{
		return View();
	}
	
	public IActionResult Feedback()
	{
		ViewData["Subjects"] = _subjects;
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Feedback(FeedbackViewModel feedbackViewModel)
	{
		if (ModelState.IsValid)
		{
			try
			{
				if (TempData.TryGetValue("CaptchaAnswer", out var answer) == false)
				{
					ModelState.AddModelError("_FORM", "Извините, не могу отправить сообщение, не работает сервис валидации Captcha");
					ViewData["subjects"] = _subjects;
					return View(feedbackViewModel);
				}

				var captchaAnswer = int.Parse(answer!.ToString()!);

				if (feedbackViewModel.CaptchaAnswer != captchaAnswer)
				{
					ModelState.AddModelError("_FORM", "Извините, неверный результат вычислений");
					
					ViewData["subjects"] = _subjects;
					return View(feedbackViewModel);
				}

				await _mediator.Publish(new FeedbackNotification(feedbackViewModel));

				TempData["Feedback"] = "Sending feedback";
				
				return RedirectToAction("FeedbackSent", "Site");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("_FORM", "Извините, не могу отправить сообщение:\n" + ex.Message);
			}
		}
		
		ViewData["subjects"] = _subjects;
		return View(feedbackViewModel);
	}

	public IActionResult FeedbackSent()
	{
		if (TempData["Feedback"] is null)
		{
			return RedirectToAction(nameof(Index), "Facts");
		}

		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
	
#pragma warning disable CA1416
	public IActionResult GetCaptchaImage(int? x, int? y, int? z)
	{
		Random r = new();
		
		x ??= r.Next(5, 10);
		y ??= r.Next(5, 10);
		z ??= r.Next(1, 5);
		
		const int imageWidth = 100;
		const int imageHeight = 30;
		using Bitmap bmp = new(imageWidth, imageHeight);
		using var g = Graphics.FromImage(bmp);

		g.SmoothingMode = SmoothingMode.AntiAlias;
		g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		g.PixelOffsetMode = PixelOffsetMode.HighQuality;
		g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

		var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		var backgroundColors = new[] { Color.BlueViolet, Color.Blue, Color.Brown, Color.DarkMagenta, Color.DarkGreen };
		var foregroundColors = new[] { Color.AliceBlue, Color.Gold, Color.GhostWhite, Color.Aqua, Color.Ivory };

		g.Clear(backgroundColors[r.Next(0, backgroundColors.Length - 1)]);
		var font = new Font("Arial", 14, FontStyle.Bold);
		var brush = new SolidBrush(foregroundColors[r.Next(0, foregroundColors.Length - 1)]);
		g.DrawString($"{x}+{y}-{z}", font, brush, new PointF(50, 15), stringFormat);
		var filename = string.Concat(_environment.WebRootPath, "/", Guid.NewGuid().ToString("N"));
		bmp.Save(filename, ImageFormat.MemoryBmp);
		byte[] bytes;
		using (FileStream stream = new(filename, FileMode.Open))
		{
			bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
		}

		System.IO.File.Delete(filename);
		TempData["CaptchaAnswer"] = x + y - z;
		return new FileContentResult(bytes, "image/jpeg");
	}
#pragma warning restore CA1416
}
