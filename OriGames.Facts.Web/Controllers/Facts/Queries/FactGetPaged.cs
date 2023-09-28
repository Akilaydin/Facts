using AutoMapper;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public class FactGetPagedRequest : OperationResultRequestBase<IPagedList<FactViewModel>>
{
	private readonly int _pageIndex;
	public int PageSize { get; set; } = 20;
	public int PageIndex {
		get => _pageIndex;
		init => _pageIndex = value - 1 < 0 ? 0 : value - 1;
	}

	public string? Tag { get; init; }
	public string? Search { get; init; }
}

public class FactGetPagedRequestHandler : OperationResultRequestHandlerBase<FactGetPagedRequest, IPagedList<FactViewModel>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	
	public FactGetPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;

	}
	
	public override async Task<OperationResult<IPagedList<FactViewModel>>> Handle(FactGetPagedRequest request, CancellationToken cancellationToken)
	{
		var operation = OperationResult.CreateResult<IPagedList<FactViewModel>>();

		var items = await _unitOfWork.GetRepository<Fact>().GetPagedListAsync(
			include: i => i.Include(x => x.Tags),
			orderBy: o => o.OrderByDescending(x => x.CreatedAt),
			pageIndex: request.PageIndex,
			pageSize: request.PageSize,
			cancellationToken: cancellationToken);

		var mappedList = _mapper.Map<IPagedList<FactViewModel>>(items);

		operation.Result = mappedList;
		operation.AddSuccess("Success");
		
		return operation;
	}
}