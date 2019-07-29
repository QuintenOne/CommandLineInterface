namespace CommandLineInterface.Applications
{
	[Application("admin")]
	public class AdminApplication
	{
		[Method("one")]
		public static string one(string name)
		{
			return "one";
		}

		[Method("two")]
		public static string two(string name, string username)
		{
			return "two";
		}

		[Method("zero")]
		public static string zero()
		{
			return "zero";
		}
	}
}