using AutoMapper;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public record RandomFactRequest : RequestBase<FactViewModel>;

public class RandomFactRequestHandler : RequestHandlerBase<RandomFactRequest, FactViewModel>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public RandomFactRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public override async Task<FactViewModel> Handle(RandomFactRequest request, CancellationToken cancellationToken)
	{
		var fact = await _unitOfWork.GetRepository<Fact>().GetAll(true).Include(f => f.Tags).OrderBy(x => EF.Functions.Random()).FirstOrDefaultAsync(cancellationToken);
		
		var mappedFact = _mapper.Map<FactViewModel>(fact);
		
		return fact == null 
			? new FactViewModel {Content = "No data"} 
			: mappedFact;
	}
}
