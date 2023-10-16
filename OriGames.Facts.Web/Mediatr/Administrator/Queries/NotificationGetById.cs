using AutoMapper;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.UnitOfWork;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Mediatr.Administrator.Queries;

public class NotificationGetByIdRequest : RequestBase<NotificationViewModel?>
{
	public NotificationGetByIdRequest(Guid id) => Id = id;

	public Guid Id { get; }
}

public class NotificationGetByIdRequestHandler : RequestHandlerBase<NotificationGetByIdRequest, NotificationViewModel?>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public NotificationGetByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public override async Task<NotificationViewModel?> Handle(NotificationGetByIdRequest request, CancellationToken cancellationToken)
	{
		var notification = await _unitOfWork
			.GetRepository<Notification>()
			.GetFirstOrDefaultAsync(
				selector: NotificationSelectors.Default, 
				predicate: x => x.Id == request.Id);

		return notification;
	}
}