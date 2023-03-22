using PinSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace PinterestTest
{
	public partial class Form1 : Form
	{
		private string AccessToken = "MTQ2NTkyMzo0NDQ1ODk5MzE4NzI5MjgwODQ6NDAyNjUzNDMyfDE2MTMyMjg0NTM6MC0tMjZmM2EzNWI5ZDcwNTFiNmExZWI5YzliYzE3ODgwMzc=";
		private string appID = "1465923";
		private string appSecret = "f955bcf0cb2b00f9184cf4d83d9000cea71b6f4e";
		private PinterestService service;
		private PinterestUser user;
		private string imageURL = "http://calendarsystems.info/DigitalUpload/Images/ThumbnailImages/0000dd59-2bdf-46b6-8934-efddfff36ab1.jpg";

		public Form1()
		{
			InitializeComponent();
			service = new PinterestService(appID, appSecret, AccessToken);
			service.OnPinterestException += Service_OnPinterestException;
			service.OnPinterestUnauthorized += Service_OnPinterestUnauthorized;
		}

		private void btnGetUser_Click(object sender, EventArgs e)
		{
			try
			{
				user = service.getUser();

				if (user != null)
				{
					btnNewBoard.Enabled = true;
					btnGetBoards.Enabled = true;
					btnGetPins.Enabled = true;
					lblUserID.Text = "Id: " + user.Id;
					lblFirstName.Text = "Full Name: " + user.FullName;
					//lblLastName.Text = "Last Name: " + user.LastName;
					lblURL.Text = "URL: " + user.ProfileURL;
				}
				else
				{
					btnNewBoard.Enabled = false;
					btnNewPin.Enabled = false;
					btnGetPins.Enabled = false;
					btnGetBoards.Enabled = false;
					lblUserID.Text = "Id: ";
					lblFirstName.Text = "First Name: ";
					lblLastName.Text = "Last Name: ";
					lblURL.Text = "URL: ";
				}
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.btnGetUser_Click(): " + E.Message);
			}
		}

		private void btnGetPins_Click(object sender, EventArgs e)
		{
			getPins();
		}

		private void getPins()
		{
			try
			{
				List<Pin> pins = service.getPins();
				lblPinCount.Text = "Pin Count: " + pins.Count;
				lbPins.DisplayMember = "Note";
				lbPins.ValueMember = "Id";
				lbPins.DataSource = pins;
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.getPins(): " + E.Message);
			}
		}

		private void lbPins_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string selectedValue = lbPins.SelectedValue.ToString();
				Pin pin = service.getPin(selectedValue);
				if (pin != null)
				{
					btnDeletePin.Enabled = true;
					lblPinID.Text = "Id: " + pin.Id;
					lblPinLink.Text = "Link: " + pin.Link;
					lblPinNote.Text = "Note: " + pin.Note;
					lblPinURL.Text = "URL: " + pin.URL;
				}
				else
				{
					btnDeletePin.Enabled = false;
					lblPinID.Text = "Id: ";
					lblPinLink.Text = "Link: ";
					lblPinNote.Text = "Note: ";
					lblPinURL.Text = "URL: ";
				}
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.lbPins_SelectedIndexChanged(): " + E.Message);
			}
		}

		private void btnGetBoards_Click(object sender, EventArgs e)
		{
			getBoards();
		}

		private void getBoards()
		{
			try
			{
				List<Board> boards = service.getBoardsForUser(user.Id);
				lblBoardCount.Text = "Board Count: " + boards.Count;
				lbBoards.DisplayMember = "Name";
				lbBoards.ValueMember = "Id";
				lbBoards.DataSource = boards;
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.getBoards(): " + E.Message);
			}
		}

		private void lbBoards_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string selectedValue = lbBoards.SelectedValue.ToString();
				Board board = service.getBoard(selectedValue);
				if (board != null)
				{
					btnDeleteBoard.Enabled = true;
					btnNewPin.Enabled = true;
					lblBoardID.Text = "Id: " + board.Id;
					lblBoardName.Text = "Name: " + board.Name;
					lblBoardURL.Text = "URL: " + board.URL;
				}
				else
				{
					btnDeleteBoard.Enabled = false;
					btnNewPin.Enabled = false;
					lblBoardID.Text = "Id: ";
					lblBoardName.Text = "Name: ";
					lblBoardURL.Text = "URL: ";
				}
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.lbPins_SelectedIndexChanged(): " + E.Message);
			}
		}

		private void btnDeletePin_Click(object sender, EventArgs e)
		{
			try
			{
				string selectedValue = lbPins.SelectedValue.ToString();
				bool ret = service.deletePin(selectedValue);
				if (ret)
				{
					btnDeletePin.Enabled = false;
					getPins();
				}
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.lbPins_SelectedIndexChanged(): " + E.Message);
			}
		}

		private void btnNewPin_Click(object sender, EventArgs e)
		{
			try
			{
				string boardID = lbBoards.SelectedValue.ToString();
				Pin pin = service.createPin(boardID, "TEST NOTE", imageURL, imageURL);
				if (pin != null)
					getPins();
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.btnNewPin_Click(): " + E.Message);
			}
		}

		private void btnNewBoard_Click(object sender, EventArgs e)
		{
			try
			{
				Board board = service.createBoard("TEST_BOARD", "description");
				if (board != null)
					getBoards();
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.btnNewBoard_Click(): " + E.Message);
			}
		}

		private void btnDeleteBoard_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show("Are you sure you want to delete this board?", "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					string boardID = lbBoards.SelectedValue.ToString();
					if (service.deleteBoard(boardID))
						getBoards();
				}
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.btnDeleteBoard_Click(): " + E.Message);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				service.OnPinterestException -= Service_OnPinterestException;
				service.OnPinterestUnauthorized -= Service_OnPinterestUnauthorized;
				service = null;

				service = new PinterestService(appID, appSecret);
				service.OnPinterestException += Service_OnPinterestException;
				service.OnPinterestUnauthorized += Service_OnPinterestUnauthorized;

				OAuthAccessToken accessToken = service.GetAccessToken(tboAuthToken.Text, tboAuthIdentifier.Text, "");
			}
			catch (Exception E)
			{
				Debug.WriteLine("Exception in Form1.btnGetAccessToken(): " + E.Message);
			}
			finally
			{
				service.OnPinterestException -= Service_OnPinterestException;
				service.OnPinterestUnauthorized -= Service_OnPinterestUnauthorized;
			}
		}

		private void Service_OnPinterestUnauthorized()
		{

		}

		private void Service_OnPinterestException(PinterestService.PinterestException ex)
		{
			Debug.WriteLine("Exception in Pinterest Service: " + ex.Message);
		}
	}
}
