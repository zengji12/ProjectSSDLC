namespace ProjectApp
{
	public static class Auth
	{
		public static void SaveToken(string token)
		{
			Properties.Settings.Default.AccessToken = token;
			Properties.Settings.Default.Save();
		}

		public static string GetToken()
		{
			return Properties.Settings.Default.AccessToken;
		}

		public static void SaveFullName(string fullname)
		{
			Properties.Settings.Default.FullName = fullname;
			Properties.Settings.Default.Save();
		}

		public static string GetFullName()
		{
			return Properties.Settings.Default.FullName;
		}

		public static void SaveAddress(string address)
		{
			Properties.Settings.Default.Alamat = address;
			Properties.Settings.Default.Save();
		}

		public static string GetAddress()
		{
			return Properties.Settings.Default.Alamat;
		}
	}
}
