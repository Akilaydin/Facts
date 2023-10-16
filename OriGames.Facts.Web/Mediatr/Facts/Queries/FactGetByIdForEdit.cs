using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Mediatr.Facts.Queries;

public record FactGetByIdForEditRequest(Guid Id, string ReturnUrl = null!) : OperationResultRequestBase<FactEditViewModel>;

public class FactGetByIdForEditRequestHandler : OperationResultRequestHandlerBase<FactGetByIdForEditRequest, FactEditViewModel>
{
	private readonly IUnitOfWork _unitOfWork;

	public FactGetByIdForEditRequestHandler(IUnitOfWork unitOfWork)
		=> _unitOfWork = unitOfWork;

	/// <summary>Handles a request</summary>
	/// <param name="request">The request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response from the request</returns>
	public override async Task<OperationResult<FactEditViewModel>> Handle(FactGetByIdForEditRequest request, CancellationToken cancellationToken)
	{
		var operation = OperationResult.CreateResult<FactEditViewModel>();

		var entity = await _unitOfWork.GetRepository<Fact>()
			.GetFirstOrDefaultAsync(
				s => new FactEditViewModel
				{
					Content = s.Content,
					Id = s.Id,
					ReturnUrl = request.ReturnUrl,
					Tags = s.Tags!.Select(x => x.Name).ToList(),
					TotalTags = s.Tags!.Count

				},
				x => x.Id == request.Id);
		if (entity is not null)
		{
			operation.Result = entity;
			return operation;
		}

		operation.AddError(new NullReferenceException($"{nameof(Fact)} not found with Id: {request.Id}"));
		return operation;
	}
}
