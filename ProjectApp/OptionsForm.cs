using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace ProjectApp
{
	public partial class OptionsForm : Form
	{
		private HttpClient _client;
		private string _baseUrl;
		public OptionsForm()
		{
			InitializeComponent();
			_client = new HttpClient();
			_baseUrl = Properties.Settings.Default.ApiBaseUrl;
		}

		private async void deleteMeButton_Click_1(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to delete your account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				try
				{
					string token = Auth.GetToken();

					if (string.IsNullOrEmpty(token))
					{
						MessageBox.Show("Token not found, please login again.");
						return;
					}

					_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

					HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/user/delete", null);

					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show("Account delete successfully!");
						foreach (Form form in Application.OpenForms)
						{
							form.Close();
						}

						Application.Restart();
					}
					else
					{
						var errorResponse = await response.Content.ReadAsStringAsync();
						var errorMessage = ErrorHandler.ExtractErrorMessage(errorResponse);
						MessageBox.Show($"Error deleting account: {response.ReasonPhrase}\nDetails: {errorMessage}");
					}

				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error deleting Account: {ex.Message}");
				}
			}
		}

		private void reNewPassButton_Click_1(object sender, EventArgs e)
		{
			ReNewPassForm renewForm = new ReNewPassForm();
			renewForm.Show();
		}

		private void backButton_Click_1(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
