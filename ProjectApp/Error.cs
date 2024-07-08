using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectApp
{
	public static class ErrorHandler
	{
		public static string ExtractErrorMessage(string jsonResponse)
		{
			try
			{
				var errorObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
				string errorMessage;

				if (errorObject.TryGetValue("message", out errorMessage))
				{
					return errorMessage;
				}
				else
				{
					return jsonResponse; // return raw response if "message" key is not found
				}
			}
			catch
			{
				return jsonResponse; // return raw response if JSON parsing fails
			}
		}
	}
}
