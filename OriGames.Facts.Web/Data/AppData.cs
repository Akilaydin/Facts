namespace OriGames.Facts.Web.Data;

public static class AppData
{
	public const string AdministratorRole = "Administrator";
	public const string UserRole = "User";

	public static IEnumerable<string> Roles {
		get {
			yield return AdministratorRole;
			yield return UserRole;
		}
	}
}
