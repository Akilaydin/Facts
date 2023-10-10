using AutoMapper;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

using MediatR;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Services;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public record FactUpdateRequest(FactEditViewModel Model) : OperationResultRequestBase<Unit>;

public class FactUpdateRequestHandler : OperationResultRequestHandlerBase<FactUpdateRequest, Unit>
{
	private readonly ITagService _tagService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public FactUpdateRequestHandler(ITagService tagService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_tagService = tagService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public override async Task<OperationResult<Unit>> Handle(FactUpdateRequest request, CancellationToken cancellationToken)
	{
		var operation = OperationResult.CreateResult<Unit>();

		var repository = _unitOfWork.GetRepository<Fact>();
		var fact = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id, include: i => i.Include(x => x.Tags), disableTracking: false);

		if (fact is null)
		{
			operation.AddError(new MicroserviceNotFoundException($"Fact with Id {request.Model.Id} not found"));
			return operation;
		}

		_mapper.Map(request.Model, fact);

		await _tagService.ProcessTagsAsync(request.Model, fact, cancellationToken);

		repository.Update(fact);

		await _unitOfWork.SaveChangesAsync();
		
		if (_unitOfWork.LastSaveChangesResult.IsOk)
		{
			operation.AddSuccess($"Fact {request.Model.Id} successfully updated");
			operation.Result = Unit.Value;
			return operation;
		}

		operation.AddError(_unitOfWork.LastSaveChangesResult.Exception);
		return operation;

	}
}
