namespace ProjectApp
{
	public class ApiKey
	{
		public string Label { get; set; }
		public string Private { get; set; }
		public ApiKey(string label, string privateKey)
		{
			Label = label;
			Private = privateKey;
		}
	}
	public static class Constants
	{
		public const string TokenNotFoundMessage = "Token not found, please login again.";
	}

}
