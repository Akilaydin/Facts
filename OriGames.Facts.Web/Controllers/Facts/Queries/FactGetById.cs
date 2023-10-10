using AutoMapper;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public class FactGetByIdRequest : OperationResultRequestBase<FactViewModel>
{
	public Guid Id { get; init; }
}

public class FactGetByIdRequestHandler : OperationResultRequestHandlerBase<FactGetByIdRequest, FactViewModel>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public FactGetByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public override async Task<OperationResult<FactViewModel>> Handle(FactGetByIdRequest request, CancellationToken cancellationToken)
	{
		var operation = OperationResult.CreateResult<FactViewModel>();
		
		operation.AppendLog("Searching fact in database");

		var entity = await _unitOfWork.GetRepository<Fact>().GetFirstOrDefaultAsync(
			predicate: f => f.Id == request.Id,
			include: i => i.Include(x => x.Tags));

		if (entity is null)
		{
			operation.AddWarning($"Fact with id {request.Id} was not found ");

			return operation;
		}
		
		operation.Result = _mapper.Map<FactViewModel>(entity);
		operation.AddSuccess($"Fact with id {request.Id} was successfully found");

		return operation;
	}
}
