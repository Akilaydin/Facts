﻿using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Mediatr.Administrator.Queries;
using OriGames.Facts.Web.Mediatr.Notifications;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

[Authorize]
public class AdministratorController : Controller
{
	private readonly IMediator _mediator;

	public AdministratorController(IMediator mediator) => _mediator = mediator;

	public IActionResult Index() => View();
	
	public IActionResult SendNotification(string subject, string mailTo, string addressFrom = "admin@gmail.com")
	{
		var model = new ManualNotificationViewModel { MailTo = mailTo, Subject = subject, MailFrom = addressFrom };
		return View(model);
	}

	[HttpPost]
	[ActionName(nameof(SendNotification))]
	public async Task<IActionResult> SendNotificationConfirm(ManualNotificationViewModel model)
	{
		await _mediator.Publish(new ManualMessageNotification(model.Subject!, model.Body!, model.MailFrom!, model.MailTo!), HttpContext.RequestAborted);
		return RedirectToAction(nameof(Notifications));
	}

	public async Task<IActionResult> Notifications(int? id, string search, bool notPrecessedOnly = false)
	{
		ViewData["NotProcessedOnly"] = notPrecessedOnly;
		var data = await _mediator.Send(new NotificationGetPagedRequest(id ?? 1, search, notPrecessedOnly), HttpContext.RequestAborted);
		return View(data);
	}

	public async Task<IActionResult> NotificationById(Guid id)
	{
		var data = await _mediator.Send(new NotificationGetByIdRequest(id), HttpContext.RequestAborted);
		return View(data);
	}
}