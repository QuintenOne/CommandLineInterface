namespace CommandLineInterface.Applications {
	[Application("customer")]
	public class CustomerApplication
	{
		[Method("a")]
		public static string multi(string name)
		{
			return "one";
		}

		[Method("b")]
		public static string multi(string name, string username)
		{
			return "two";
		}

		[Method("c")]
		public static string multi()
		{
			return "zero";
		}
	}
}
