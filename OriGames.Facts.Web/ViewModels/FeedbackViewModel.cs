using System.ComponentModel.DataAnnotations;

namespace OriGames.Facts.Web.ViewModels;

public class FeedbackViewModel
{
	[Required(ErrorMessage = "{0} - обязательное поле")]
	[StringLength(100, ErrorMessage = "Длина {0} не должна превышать {1} символов")]
	[Display(Name = "Тема сообщения")]
	public string Subject { get; set; } = null!;

	[Required(ErrorMessage = "{0} - обязательное поле")]
	[StringLength(50, ErrorMessage = "Длина {0} не должна превышать {1} символов")]
	[Display(Name = "Имя")]
	public string UserName { get; set; } = null!;

	[Required(ErrorMessage = "{0} - обязательное поле")]
	[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "{0} - неверный формат")]
	[StringLength(50, ErrorMessage = "Длина {0} не должна превышать {1} символов")]
	[Display(Name = "Email")]
	public string MailFrom { get; set; } = null!;

	[Required(ErrorMessage = "{0} - обязательное поле")]
	[StringLength(500, ErrorMessage = "Длина {0} не должна превышать {1} символов")]
	[DataType(DataType.MultilineText)]
	[Display(Name = "Текст сообщения")]
	public string Body { get; set; } = null!;
}
