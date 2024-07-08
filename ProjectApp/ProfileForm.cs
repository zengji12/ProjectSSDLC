using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectApp
{
	public partial class ProfileForm : Form
	{
		public ProfileForm(string name, string address)
		{
			InitializeComponent();
			labelNamaUser.Text = name;
			labelAlamatUser.Text = address;
		}

		private void backButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
