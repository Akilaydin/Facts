using System.Text;

namespace OriGames.Facts.Infrastructure.Services;

public class EncodingStringWriterService : StringWriter
{
	public override Encoding Encoding { get; }
	
	public EncodingStringWriterService(Encoding encoding)
	{
		Encoding = encoding;
	}
}
