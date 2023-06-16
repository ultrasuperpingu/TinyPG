using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyPG.Controls
{
	public partial class SearchDialog : Form
	{
		public SearchDialog()
		{
			InitializeComponent();
		}
		private void searchCancelBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void searchNextBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
		public string SearchText
		{
			get { return this.textBox1.Text; }
			set { this.textBox1.Text = value; }
		}
	}
}
