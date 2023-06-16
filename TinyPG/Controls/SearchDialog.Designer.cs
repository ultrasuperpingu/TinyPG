namespace TinyPG.Controls
{
	partial class SearchDialog
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.searchNextBtn = new System.Windows.Forms.Button();
			this.searchCancelBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(191, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(334, 22);
			this.textBox1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Text to search:";
			// 
			// searchNextBtn
			// 
			this.searchNextBtn.Location = new System.Drawing.Point(317, 85);
			this.searchNextBtn.Name = "searchNextBtn";
			this.searchNextBtn.Size = new System.Drawing.Size(90, 23);
			this.searchNextBtn.TabIndex = 2;
			this.searchNextBtn.Text = "Next";
			this.searchNextBtn.UseVisualStyleBackColor = true;
			this.searchNextBtn.Click += new System.EventHandler(this.searchNextBtn_Click);
			// 
			// searchCancelBtn
			// 
			this.searchCancelBtn.Location = new System.Drawing.Point(439, 85);
			this.searchCancelBtn.Name = "searchCancelBtn";
			this.searchCancelBtn.Size = new System.Drawing.Size(90, 23);
			this.searchCancelBtn.TabIndex = 3;
			this.searchCancelBtn.Text = "Cancel";
			this.searchCancelBtn.UseVisualStyleBackColor = true;
			this.searchCancelBtn.Click += new System.EventHandler(this.searchCancelBtn_Click);
			// 
			// SearchDialog
			// 
			this.AcceptButton = this.searchNextBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.searchCancelBtn;
			this.ClientSize = new System.Drawing.Size(537, 120);
			this.Controls.Add(this.searchCancelBtn);
			this.Controls.Add(this.searchNextBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SearchDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Search";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button searchNextBtn;
		private System.Windows.Forms.Button searchCancelBtn;
	}
}