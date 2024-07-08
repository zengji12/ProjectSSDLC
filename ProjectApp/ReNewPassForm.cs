using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectApp
{
	public partial class ReNewPassForm : Form
	{
		private readonly HttpClient _client;
		private readonly string _baseUrl;

		public ReNewPassForm()
		{
			InitializeComponent();
			_client = new HttpClient();
			_baseUrl = Properties.Settings.Default.ApiBaseUrl;
		}

		private async void submitPass_Click(object sender, EventArgs e)
		{
			string oldPass = OldTextBox.Text;
			string newPass = NewTextBox.Text;

			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show("Token not found, please login again.");
					return;
				}

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var values = new { password = oldPass, newPassword = newPass };
				var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/auth/changePassword", content);

				response.EnsureSuccessStatusCode();

				if (response.IsSuccessStatusCode)
				{
					MessageBox.Show("Password changed successfully!");
					backButton_Click(sender, e);
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					var errorMessage = ErrorHandler.ExtractErrorMessage(errorResponse);
					MessageBox.Show($"Error changing password: {response.ReasonPhrase}\nDetails: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error changing password: {ex.Message}");
			}
		}

		private void backButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
