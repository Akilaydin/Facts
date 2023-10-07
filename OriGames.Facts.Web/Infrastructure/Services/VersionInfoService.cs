namespace OriGames.Facts.Web.Infrastructure.Services;

public class VersionInfoService : IVersionInfoService
{
	string IVersionInfoService.Version => ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch;

	string IVersionInfoService.Branch => ThisAssembly.Git.Branch;

	string IVersionInfoService.Commit => ThisAssembly.Git.Commit;
}
