namespace ProjectApp
{
	partial class ProfileForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			backButton = new Button();
			labelNama = new Label();
			labelAlamat = new Label();
			labelNamaUser = new Label();
			labelAlamatUser = new Label();
			SuspendLayout();
			// 
			// backButton
			// 
			backButton.Location = new Point(222, 165);
			backButton.Name = "backButton";
			backButton.Size = new Size(75, 23);
			backButton.TabIndex = 0;
			backButton.Text = "Back";
			backButton.UseVisualStyleBackColor = true;
			backButton.Click += backButton_Click;
			// 
			// labelNama
			// 
			labelNama.AutoSize = true;
			labelNama.Font = new Font("Segoe UI", 18F);
			labelNama.Location = new Point(12, 40);
			labelNama.Name = "labelNama";
			labelNama.Size = new Size(77, 32);
			labelNama.TabIndex = 1;
			labelNama.Text = "Nama";
			// 
			// labelAlamat
			// 
			labelAlamat.AutoSize = true;
			labelAlamat.Font = new Font("Segoe UI", 18F);
			labelAlamat.Location = new Point(12, 98);
			labelAlamat.Name = "labelAlamat";
			labelAlamat.Size = new Size(88, 32);
			labelAlamat.TabIndex = 2;
			labelAlamat.Text = "Alamat";
			// 
			// labelNamaUser
			// 
			labelNamaUser.AutoSize = true;
			labelNamaUser.Font = new Font("Segoe UI", 18F);
			labelNamaUser.Location = new Point(120, 40);
			labelNamaUser.Name = "labelNamaUser";
			labelNamaUser.Size = new Size(77, 32);
			labelNamaUser.TabIndex = 3;
			labelNamaUser.Text = "Nama";
			// 
			// labelAlamatUser
			// 
			labelAlamatUser.AutoSize = true;
			labelAlamatUser.Font = new Font("Segoe UI", 18F);
			labelAlamatUser.Location = new Point(120, 98);
			labelAlamatUser.Name = "labelAlamatUser";
			labelAlamatUser.Size = new Size(88, 32);
			labelAlamatUser.TabIndex = 4;
			labelAlamatUser.Text = "Alamat";
			// 
			// ProfileForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(523, 210);
			Controls.Add(labelAlamatUser);
			Controls.Add(labelNamaUser);
			Controls.Add(labelAlamat);
			Controls.Add(labelNama);
			Controls.Add(backButton);
			Name = "ProfileForm";
			Text = "ProfileForm";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button backButton;
		private Label labelNama;
		private Label labelAlamat;
		private Label labelNamaUser;
		private Label labelAlamatUser;
	}
}
