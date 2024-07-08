using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ProjectApp
{
	public partial class DeleteUserForm : Form
	{
		private HttpClient _client;
		private string _baseUrl;
		public DeleteUserForm()
		{
			InitializeComponent();
			_client = new HttpClient();
			_baseUrl = Properties.Settings.Default.ApiBaseUrl;
		}

		private async void buttonDelete_Click(object sender, EventArgs e)
		{
			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show("Token not found, please login again.");
					return;
				}

				string userId = userIdTxt.Text;

				var values = new
				{
					userId = userId
				};

				var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/auth/deleteUser", content);

				if (response.IsSuccessStatusCode)
				{
					MessageBox.Show("User deleted successfully!");
					this.Close();
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					var errorMessage = ErrorHandler.ExtractErrorMessage(errorResponse);
					MessageBox.Show($"Error deleting user: {response.ReasonPhrase}\nDetails: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred: {ex.Message}");
			}
		}

		private void buttonBack_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
