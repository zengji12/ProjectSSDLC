using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ProjectApp
{
	public partial class LoginForm : Form
	{
		private static HttpClient _client = new HttpClient();
		private string _baseUrl;

		public LoginForm()
		{
			InitializeComponent();
			_client = new HttpClient();
			_baseUrl = Properties.Settings.Default.ApiBaseUrl;
		}

		private async void login_Button_1(object sender, EventArgs e)
		{
			string username = usernameTxt.Text;
			string password = passwordTxt.Text;
			var loginResult = await LoginAsync(username, password);

			if (loginResult != null && !string.IsNullOrEmpty(loginResult.accessToken))
			{
				Auth.SaveToken(loginResult.accessToken);
				Auth.SaveFullName(loginResult.name);
				Auth.SaveAddress(loginResult.address);
				MessageBox.Show("Login successful!");

				DashboardForm dashboardForm = new DashboardForm(Auth.GetFullName());
				dashboardForm.Show();
				this.Hide();
			}
			else
			{
				MessageBox.Show("Login failed!");
			}
		}
		private async Task<LoginResponse> LoginAsync(string username, string password)
		{
			try
			{
				var content = new StringContent(JsonConvert.SerializeObject(new { username, password }), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/auth/signin", content);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<LoginResponse>(responseBody);
			}
			catch (HttpRequestException ex)
			{
				if (ex.StatusCode == HttpStatusCode.Unauthorized)
				{
					MessageBox.Show("Unauthorized: Username or password is incorrect.");
				}
				else if (ex.StatusCode == HttpStatusCode.BadRequest)
				{
					MessageBox.Show("Bad Request: User unavailable.");
				}
				else
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
				return null;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred: {ex.Message}");
				return null;
			}
		}
	}

		public class LoginResponse
	{
		public string? name { get; set; }
		public string? accessToken { get; set; }
		public string? address { get; set; }
	}
}
