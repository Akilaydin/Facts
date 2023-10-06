﻿using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;

namespace OriGames.Facts.Contracts;

public class TagSearchService : ITagSearchService
{
	private readonly IUnitOfWork _unitOfWork;

	public TagSearchService(IUnitOfWork unitOfWork) {
		_unitOfWork = unitOfWork;
	}

	public List<string> SearchTags(string term)
	{
		var tags = _unitOfWork.GetRepository<Tag>()
			.GetAll(s => s.Name, x => x.Name.ToLower()
			.StartsWith(term.ToLower()), true).ToList();

		return tags;
	}
}
