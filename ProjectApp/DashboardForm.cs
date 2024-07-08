using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectApp
{
	public partial class DashboardForm : Form
	{
		private string fullname;
		private HttpClient _client;
		private string _baseUrl;
		private bool isAdmin;

		public DashboardForm(string fullname)
		{
			InitializeComponent();
			this.fullname = fullname;
			_client = new HttpClient();
			_baseUrl = Properties.Settings.Default.ApiBaseUrl;
			isAdmin = false;
		}

		private async void Dashboard_Load(object sender, EventArgs e)
		{
			label1.Text = $"Selamat Datang {fullname}";
			await CheckAdminStatus();
			buttonAdminRegister.Visible = isAdmin;
			buttonAdminDelete.Visible = isAdmin;
			await LoadUserKeysAsync();
		}

		private async Task LoadUserKeysAsync()
		{
			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show(Constants.TokenNotFoundMessage);
					return;
				}

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var response = await _client.PostAsync($"{_baseUrl}api/user/getkey", null);

				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Response: {content}");

				if (string.IsNullOrEmpty(content))
				{
					MessageBox.Show("No keys found for the user.");
					return;
				}

				var keys = JsonConvert.DeserializeObject<ApiKey[]>(content);

				dataGridViewKeys.Columns.Clear();
				dataGridViewKeys.DataSource = null;

				dataGridViewKeys.Columns.Add("Label", "Label");
				dataGridViewKeys.Columns.Add("Private", "Private");

				dataGridViewKeys.Columns[0].DataPropertyName = "Label";
				dataGridViewKeys.Columns[1].DataPropertyName = "Private";

				dataGridViewKeys.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewKeys.Columns[0].Width = 100;
				dataGridViewKeys.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				dataGridViewKeys.DataSource = keys;
			}
			catch (HttpRequestException ex)
			{
				MessageBox.Show($"Error fetching user keys: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred: {ex.Message}");
			}
		}

		private async Task CheckAdminStatus()
		{
			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show(Constants.TokenNotFoundMessage);
					return;
				}

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var response = await _client.PostAsync($"{_baseUrl}api/auth/isadmin", null);

				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<dynamic>(content);
				if (result != null)
				{
					isAdmin = result.isAdmin;
				}
				else
				{
					MessageBox.Show("Response from server was null.");
					return;
				}
			}
			catch (HttpRequestException ex)
			{
				MessageBox.Show($"Error checking admin status: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred: {ex.Message}");
			}
		}
		private async void buttonNew_Click(object sender, EventArgs e)
		{
			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show(Constants.TokenNotFoundMessage);
					return;
				}

				string label = ShowInputDialog("Enter Label", null);
				if (string.IsNullOrEmpty(label))
				{
					MessageBox.Show("Label cannot be empty.");
					return;
				}

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var values = new { label = label };
				var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/user/newkey", content);

				if (response.IsSuccessStatusCode)
				{
					MessageBox.Show("Key created successfully!");
					await LoadUserKeysAsync();
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					var errorMessage = ErrorHandler.ExtractErrorMessage(errorResponse);
					MessageBox.Show($"Error creating key: {response.ReasonPhrase}\nDetails: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error creating key: {ex.Message}");
			}
		}


		private string ShowInputDialog(string caption, string defaultValue)
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 220,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Text = caption };
			TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text = defaultValue };
			Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 90, DialogResult = DialogResult.OK };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(textLabel);
			prompt.Controls.Add(textBox);
			prompt.AcceptButton = confirmation;

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
		}

		private async void buttonDelete_Click(object sender, EventArgs e)
		{
			try
			{
				string token = Auth.GetToken();

				if (string.IsNullOrEmpty(token))
				{
					MessageBox.Show(Constants.TokenNotFoundMessage);
					return;
				}

				string label = ShowInputDialog("Enter Label", null);
				if (string.IsNullOrEmpty(label))
				{
					MessageBox.Show("Label cannot be empty.");
					return;
				}

				string privateKey = GetPrivateKeyByLabel(label);
				if (string.IsNullOrEmpty(privateKey))
				{
					MessageBox.Show("Private key not found for the given label.");
					return;
				}

				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var values = new { label = label, privateKey = privateKey };
				var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _client.PostAsync($"{_baseUrl}api/user/deletekey", content);

				if (response.IsSuccessStatusCode)
				{
					MessageBox.Show("Key deleted successfully!");
					await LoadUserKeysAsync();
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					var errorMessage = ErrorHandler.ExtractErrorMessage(errorResponse);
					MessageBox.Show($"Error deleting key: {response.ReasonPhrase}\nDetails: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error deleting key: {ex.Message}");
			}
		}

		private string GetPrivateKeyByLabel(string label)
		{
			foreach (DataGridViewRow row in dataGridViewKeys.Rows)
			{
				var cellValue = row.Cells["Label"].Value?.ToString();
				if (cellValue == label)
				{
					var privateValue = row.Cells["Private"].Value?.ToString();
					if (privateValue != null)
					{
						return privateValue;
					}
					else
					{
						return string.Empty;
					}
				}
			}
			return string.Empty;
		}

		private void buttonOptions_Click(object sender, EventArgs e)
		{
			OptionsForm optionsForm = new OptionsForm();
			optionsForm.ShowDialog();
		}

		private void buttonAdminRegister_Click(object sender, EventArgs e)
		{
			if (isAdmin)
			{
				RegistrationForm registrationForm = new RegistrationForm();
				registrationForm.Show();
			}
			else
			{
				MessageBox.Show("You do not have permission to access this feature.");
			}
		}

		private void buttonAdminDelete_Click(object sender, EventArgs e)
		{
			if (isAdmin)
			{
				DeleteUserForm deleteUserForm = new DeleteUserForm();
				deleteUserForm.Show();
			}
			else
			{
				MessageBox.Show("You do not have permission to access this feature.");
			}
		}

		private void profileButton_Click(object sender, EventArgs e)
		{
			ProfileForm profileForm = new ProfileForm(Auth.GetFullName(), Auth.GetAddress());
			profileForm.Show();
		}
	}
}
