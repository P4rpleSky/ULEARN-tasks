namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			string res;
			switch (count % 10)
			{
				case 1:
					res = "рубль";
					break;
				case 2:
					res = "рубля";
					break;
				case 3:
					res = "рубля";
					break;
				case 4:
					res = "рубля";
					break;

				default:
					res = "рублей";
					break;
			}
			if (count % 100 > 10 && count % 100 < 20)
				res = "рублей";
			return res;
		}
	}
}