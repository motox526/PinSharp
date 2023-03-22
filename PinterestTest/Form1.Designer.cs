namespace PinterestTest
{
	partial class Form1
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
			this.btnGetUser = new System.Windows.Forms.Button();
			this.btnGetPins = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblURL = new System.Windows.Forms.Label();
			this.lblLastName = new System.Windows.Forms.Label();
			this.lblFirstName = new System.Windows.Forms.Label();
			this.lblUserID = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lbPins = new System.Windows.Forms.ListBox();
			this.lblPinCount = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnDeletePin = new System.Windows.Forms.Button();
			this.lblPinNote = new System.Windows.Forms.Label();
			this.lblPinLink = new System.Windows.Forms.Label();
			this.lblPinURL = new System.Windows.Forms.Label();
			this.lblPinID = new System.Windows.Forms.Label();
			this.btnGetBoards = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lbBoards = new System.Windows.Forms.ListBox();
			this.lblBoardCount = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.btnDeleteBoard = new System.Windows.Forms.Button();
			this.lblBoardName = new System.Windows.Forms.Label();
			this.lblBoardURL = new System.Windows.Forms.Label();
			this.lblBoardID = new System.Windows.Forms.Label();
			this.btnNewPin = new System.Windows.Forms.Button();
			this.btnNewBoard = new System.Windows.Forms.Button();
			this.btnGetAccessToken = new System.Windows.Forms.Button();
			this.tboAuthToken = new System.Windows.Forms.TextBox();
			this.tboAuthIdentifier = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnGetUser
			// 
			this.btnGetUser.Location = new System.Drawing.Point(20, 10);
			this.btnGetUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnGetUser.Name = "btnGetUser";
			this.btnGetUser.Size = new System.Drawing.Size(84, 25);
			this.btnGetUser.TabIndex = 0;
			this.btnGetUser.Text = "Get User";
			this.btnGetUser.UseVisualStyleBackColor = true;
			this.btnGetUser.Click += new System.EventHandler(this.btnGetUser_Click);
			// 
			// btnGetPins
			// 
			this.btnGetPins.Enabled = false;
			this.btnGetPins.Location = new System.Drawing.Point(20, 47);
			this.btnGetPins.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnGetPins.Name = "btnGetPins";
			this.btnGetPins.Size = new System.Drawing.Size(84, 25);
			this.btnGetPins.TabIndex = 1;
			this.btnGetPins.Text = "Get Pins";
			this.btnGetPins.UseVisualStyleBackColor = true;
			this.btnGetPins.Click += new System.EventHandler(this.btnGetPins_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblURL);
			this.groupBox1.Controls.Add(this.lblLastName);
			this.groupBox1.Controls.Add(this.lblFirstName);
			this.groupBox1.Controls.Add(this.lblUserID);
			this.groupBox1.Location = new System.Drawing.Point(150, 10);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Size = new System.Drawing.Size(332, 91);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "User Data";
			// 
			// lblURL
			// 
			this.lblURL.AutoSize = true;
			this.lblURL.Location = new System.Drawing.Point(14, 71);
			this.lblURL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(32, 13);
			this.lblURL.TabIndex = 3;
			this.lblURL.Text = "URL:";
			// 
			// lblLastName
			// 
			this.lblLastName.AutoSize = true;
			this.lblLastName.Location = new System.Drawing.Point(14, 52);
			this.lblLastName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLastName.Name = "lblLastName";
			this.lblLastName.Size = new System.Drawing.Size(61, 13);
			this.lblLastName.TabIndex = 2;
			this.lblLastName.Text = "Last Name:";
			// 
			// lblFirstName
			// 
			this.lblFirstName.AutoSize = true;
			this.lblFirstName.Location = new System.Drawing.Point(14, 35);
			this.lblFirstName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblFirstName.Name = "lblFirstName";
			this.lblFirstName.Size = new System.Drawing.Size(60, 13);
			this.lblFirstName.TabIndex = 1;
			this.lblFirstName.Text = "First Name:";
			// 
			// lblUserID
			// 
			this.lblUserID.AutoSize = true;
			this.lblUserID.Location = new System.Drawing.Point(14, 17);
			this.lblUserID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblUserID.Name = "lblUserID";
			this.lblUserID.Size = new System.Drawing.Size(22, 13);
			this.lblUserID.TabIndex = 0;
			this.lblUserID.Text = "Id: ";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lbPins);
			this.groupBox2.Controls.Add(this.lblPinCount);
			this.groupBox2.Location = new System.Drawing.Point(150, 113);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Size = new System.Drawing.Size(332, 151);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Pins";
			// 
			// lbPins
			// 
			this.lbPins.FormattingEnabled = true;
			this.lbPins.Location = new System.Drawing.Point(16, 31);
			this.lbPins.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.lbPins.Name = "lbPins";
			this.lbPins.Size = new System.Drawing.Size(312, 108);
			this.lbPins.TabIndex = 2;
			this.lbPins.SelectedIndexChanged += new System.EventHandler(this.lbPins_SelectedIndexChanged);
			// 
			// lblPinCount
			// 
			this.lblPinCount.AutoSize = true;
			this.lblPinCount.Location = new System.Drawing.Point(14, 15);
			this.lblPinCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPinCount.Name = "lblPinCount";
			this.lblPinCount.Size = new System.Drawing.Size(59, 13);
			this.lblPinCount.TabIndex = 1;
			this.lblPinCount.Text = "Pin Count: ";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnDeletePin);
			this.groupBox3.Controls.Add(this.lblPinNote);
			this.groupBox3.Controls.Add(this.lblPinLink);
			this.groupBox3.Controls.Add(this.lblPinURL);
			this.groupBox3.Controls.Add(this.lblPinID);
			this.groupBox3.Location = new System.Drawing.Point(486, 113);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox3.Size = new System.Drawing.Size(409, 151);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Pin Data";
			// 
			// btnDeletePin
			// 
			this.btnDeletePin.Enabled = false;
			this.btnDeletePin.Location = new System.Drawing.Point(320, 9);
			this.btnDeletePin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnDeletePin.Name = "btnDeletePin";
			this.btnDeletePin.Size = new System.Drawing.Size(84, 25);
			this.btnDeletePin.TabIndex = 9;
			this.btnDeletePin.Text = "Delete Pin";
			this.btnDeletePin.UseVisualStyleBackColor = true;
			this.btnDeletePin.Click += new System.EventHandler(this.btnDeletePin_Click);
			// 
			// lblPinNote
			// 
			this.lblPinNote.Location = new System.Drawing.Point(4, 66);
			this.lblPinNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPinNote.Name = "lblPinNote";
			this.lblPinNote.Size = new System.Drawing.Size(227, 80);
			this.lblPinNote.TabIndex = 7;
			this.lblPinNote.Text = "Note: ";
			// 
			// lblPinLink
			// 
			this.lblPinLink.AutoSize = true;
			this.lblPinLink.Location = new System.Drawing.Point(4, 49);
			this.lblPinLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPinLink.Name = "lblPinLink";
			this.lblPinLink.Size = new System.Drawing.Size(33, 13);
			this.lblPinLink.TabIndex = 6;
			this.lblPinLink.Text = "Link: ";
			// 
			// lblPinURL
			// 
			this.lblPinURL.AutoSize = true;
			this.lblPinURL.Location = new System.Drawing.Point(4, 32);
			this.lblPinURL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPinURL.Name = "lblPinURL";
			this.lblPinURL.Size = new System.Drawing.Size(35, 13);
			this.lblPinURL.TabIndex = 5;
			this.lblPinURL.Text = "URL: ";
			// 
			// lblPinID
			// 
			this.lblPinID.AutoSize = true;
			this.lblPinID.Location = new System.Drawing.Point(4, 15);
			this.lblPinID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPinID.Name = "lblPinID";
			this.lblPinID.Size = new System.Drawing.Size(22, 13);
			this.lblPinID.TabIndex = 4;
			this.lblPinID.Text = "Id: ";
			// 
			// btnGetBoards
			// 
			this.btnGetBoards.Enabled = false;
			this.btnGetBoards.Location = new System.Drawing.Point(20, 88);
			this.btnGetBoards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnGetBoards.Name = "btnGetBoards";
			this.btnGetBoards.Size = new System.Drawing.Size(84, 25);
			this.btnGetBoards.TabIndex = 5;
			this.btnGetBoards.Text = "Get Boards";
			this.btnGetBoards.UseVisualStyleBackColor = true;
			this.btnGetBoards.Click += new System.EventHandler(this.btnGetBoards_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.lbBoards);
			this.groupBox4.Controls.Add(this.lblBoardCount);
			this.groupBox4.Location = new System.Drawing.Point(150, 279);
			this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox4.Size = new System.Drawing.Size(332, 151);
			this.groupBox4.TabIndex = 4;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Boards";
			// 
			// lbBoards
			// 
			this.lbBoards.FormattingEnabled = true;
			this.lbBoards.Location = new System.Drawing.Point(16, 31);
			this.lbBoards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.lbBoards.Name = "lbBoards";
			this.lbBoards.Size = new System.Drawing.Size(312, 108);
			this.lbBoards.TabIndex = 2;
			this.lbBoards.SelectedIndexChanged += new System.EventHandler(this.lbBoards_SelectedIndexChanged);
			// 
			// lblBoardCount
			// 
			this.lblBoardCount.AutoSize = true;
			this.lblBoardCount.Location = new System.Drawing.Point(14, 15);
			this.lblBoardCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblBoardCount.Name = "lblBoardCount";
			this.lblBoardCount.Size = new System.Drawing.Size(72, 13);
			this.lblBoardCount.TabIndex = 1;
			this.lblBoardCount.Text = "Board Count: ";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.btnDeleteBoard);
			this.groupBox5.Controls.Add(this.lblBoardName);
			this.groupBox5.Controls.Add(this.lblBoardURL);
			this.groupBox5.Controls.Add(this.lblBoardID);
			this.groupBox5.Location = new System.Drawing.Point(486, 279);
			this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox5.Size = new System.Drawing.Size(409, 151);
			this.groupBox5.TabIndex = 8;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Board Data";
			// 
			// btnDeleteBoard
			// 
			this.btnDeleteBoard.Enabled = false;
			this.btnDeleteBoard.Location = new System.Drawing.Point(320, 9);
			this.btnDeleteBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnDeleteBoard.Name = "btnDeleteBoard";
			this.btnDeleteBoard.Size = new System.Drawing.Size(84, 25);
			this.btnDeleteBoard.TabIndex = 10;
			this.btnDeleteBoard.Text = "Delete Board";
			this.btnDeleteBoard.UseVisualStyleBackColor = true;
			this.btnDeleteBoard.Click += new System.EventHandler(this.btnDeleteBoard_Click);
			// 
			// lblBoardName
			// 
			this.lblBoardName.AutoSize = true;
			this.lblBoardName.Location = new System.Drawing.Point(4, 49);
			this.lblBoardName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblBoardName.Name = "lblBoardName";
			this.lblBoardName.Size = new System.Drawing.Size(38, 13);
			this.lblBoardName.TabIndex = 6;
			this.lblBoardName.Text = "Name:";
			// 
			// lblBoardURL
			// 
			this.lblBoardURL.AutoSize = true;
			this.lblBoardURL.Location = new System.Drawing.Point(4, 32);
			this.lblBoardURL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblBoardURL.Name = "lblBoardURL";
			this.lblBoardURL.Size = new System.Drawing.Size(35, 13);
			this.lblBoardURL.TabIndex = 5;
			this.lblBoardURL.Text = "URL: ";
			// 
			// lblBoardID
			// 
			this.lblBoardID.AutoSize = true;
			this.lblBoardID.Location = new System.Drawing.Point(4, 15);
			this.lblBoardID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblBoardID.Name = "lblBoardID";
			this.lblBoardID.Size = new System.Drawing.Size(22, 13);
			this.lblBoardID.TabIndex = 4;
			this.lblBoardID.Text = "Id: ";
			// 
			// btnNewPin
			// 
			this.btnNewPin.Enabled = false;
			this.btnNewPin.Location = new System.Drawing.Point(20, 145);
			this.btnNewPin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnNewPin.Name = "btnNewPin";
			this.btnNewPin.Size = new System.Drawing.Size(84, 25);
			this.btnNewPin.TabIndex = 9;
			this.btnNewPin.Text = "New Pin";
			this.btnNewPin.UseVisualStyleBackColor = true;
			this.btnNewPin.Click += new System.EventHandler(this.btnNewPin_Click);
			// 
			// btnNewBoard
			// 
			this.btnNewBoard.Enabled = false;
			this.btnNewBoard.Location = new System.Drawing.Point(20, 293);
			this.btnNewBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnNewBoard.Name = "btnNewBoard";
			this.btnNewBoard.Size = new System.Drawing.Size(84, 25);
			this.btnNewBoard.TabIndex = 10;
			this.btnNewBoard.Text = "New Board";
			this.btnNewBoard.UseVisualStyleBackColor = true;
			this.btnNewBoard.Click += new System.EventHandler(this.btnNewBoard_Click);
			// 
			// btnGetAccessToken
			// 
			this.btnGetAccessToken.Location = new System.Drawing.Point(772, 61);
			this.btnGetAccessToken.Margin = new System.Windows.Forms.Padding(2);
			this.btnGetAccessToken.Name = "btnGetAccessToken";
			this.btnGetAccessToken.Size = new System.Drawing.Size(118, 25);
			this.btnGetAccessToken.TabIndex = 11;
			this.btnGetAccessToken.Text = "Get Access Token";
			this.btnGetAccessToken.UseVisualStyleBackColor = true;
			this.btnGetAccessToken.Click += new System.EventHandler(this.button1_Click);
			// 
			// tboAuthToken
			// 
			this.tboAuthToken.Location = new System.Drawing.Point(697, 10);
			this.tboAuthToken.Name = "tboAuthToken";
			this.tboAuthToken.Size = new System.Drawing.Size(193, 20);
			this.tboAuthToken.TabIndex = 12;
			// 
			// tboAuthIdentifier
			// 
			this.tboAuthIdentifier.Location = new System.Drawing.Point(697, 36);
			this.tboAuthIdentifier.Name = "tboAuthIdentifier";
			this.tboAuthIdentifier.Size = new System.Drawing.Size(193, 20);
			this.tboAuthIdentifier.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(620, 13);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "oAuth Token:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(611, 39);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "oAuth Identifier:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(904, 448);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tboAuthIdentifier);
			this.Controls.Add(this.tboAuthToken);
			this.Controls.Add(this.btnGetAccessToken);
			this.Controls.Add(this.btnNewBoard);
			this.Controls.Add(this.btnNewPin);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnGetBoards);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnGetPins);
			this.Controls.Add(this.btnGetUser);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGetUser;
		private System.Windows.Forms.Button btnGetPins;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblURL;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblUserID;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblPinCount;
		private System.Windows.Forms.ListBox lbPins;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label lblPinNote;
		private System.Windows.Forms.Label lblPinLink;
		private System.Windows.Forms.Label lblPinURL;
		private System.Windows.Forms.Label lblPinID;
		private System.Windows.Forms.Button btnGetBoards;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ListBox lbBoards;
		private System.Windows.Forms.Label lblBoardCount;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label lblBoardName;
		private System.Windows.Forms.Label lblBoardURL;
		private System.Windows.Forms.Label lblBoardID;
		private System.Windows.Forms.Button btnDeletePin;
		private System.Windows.Forms.Button btnNewPin;
		private System.Windows.Forms.Button btnNewBoard;
		private System.Windows.Forms.Button btnDeleteBoard;
		private System.Windows.Forms.Button btnGetAccessToken;
		private System.Windows.Forms.TextBox tboAuthToken;
		private System.Windows.Forms.TextBox tboAuthIdentifier;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

