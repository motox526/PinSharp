using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

/*Copyright © 2023, Chris Butterfield Software Solutions, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither the name of Chris Butterfield Software Solutions, LLC, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written
permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/

namespace PinSharp
{
	public partial class PinterestService
	{
		#region Private Class Data
		private string _appID;
		private string _appSecret;
		private string _token;
		//private readonly RestClient _client;
		#endregion

		public delegate void PinterestExceptionDelegate(PinterestException ex);
		public delegate void PinterestUnauthorizedDelegate();
		public delegate void PinterestMessageDelegate(string message);

		public class PinterestException : Exception
		{
			public PinterestException(string message)
			: base(message)
			{ }
		}

		/// <summary>
		/// Notifies subscribers of a Pinterest Exception
		/// </summary>
		public event PinterestExceptionDelegate OnPinterestException = null;

		public event PinterestUnauthorizedDelegate OnPinterestUnauthorized = null;

		/// <summary>
		/// Notifies subscribers of a pinterest message
		/// </summary>
		public event PinterestMessageDelegate OnPinterestMessage = null;

		#region Constructors
		/// <summary>
		/// Creates a new instance of a PinterestService
		/// </summary>
		/// <param name="appID">The App ID</param>
		/// <param name="appSecret">The App Secret</param>
		public PinterestService(string appID, string appSecret)
			: this()
		{
			_appID = appID;
			_appSecret = appSecret;
		}

		/// <summary>
		/// Creates a new instance of a PinterestService
		/// </summary>
		/// <param name="appID">The App ID</param>
		/// <param name="appSecret">The App Secret</param>
		/// <param name="accessToken">The Access Token</param>
		public PinterestService(string appID, string appSecret, string accessToken)
			: this(appID, appSecret)
		{
			_token = accessToken;
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public PinterestService()
		{
			_oauth = new RestClient
			{
				Authority = Globals.PinterestBaseURL,
				UserAgent = "PinSharp",
				DecompressionMethods = DecompressionMethods.GZip,
				GetErrorResponseEntityType = (request, @base) => typeof(PinterestError),
			};
		}
		#endregion

		#region Getters/Setters
		/// <summary>
		/// Gets or sets the Response object
		/// </summary>
		public virtual PinterestResponse Response { get; private set; }
		#endregion

		#region Queries
		private readonly Func<FunctionArguments, RestRequest> _userQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/users/" + args.Username + "/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _pinsQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/users/me/pins/" + (string.IsNullOrEmpty(args.ObjectID) ? "" : (args.ObjectID + "/"))
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _boardsQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/users/" + args.UserID + "/boards/feed/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _createBoardQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Post,
					Path = "/" + Globals.PinterestAPIVersion + "/boards/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _pinQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/pins/" + args.ObjectID + "/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _createPinQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Put,
					Path = "/" + Globals.PinterestAPIVersion + "/pins/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _deletePinQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Delete,
					Path = "/" + Globals.PinterestAPIVersion + "/pins/" + args.ObjectID + "/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _boardQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/boards/" + args.ObjectID + "/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _deleteBoardQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Delete,
					Path = "/" + Globals.PinterestAPIVersion + "/boards/" + args.ObjectID + "/"
				};
				return request;
			};

		private readonly Func<FunctionArguments, RestRequest> _boardPinsQuery
			= args =>
			{
				var request = new RestRequest
				{
					Credentials = new OAuthCredentials
					{
						ConsumerKey = args.AppID,
						ConsumerSecret = args.AppSecret,
						Token = args.Token,
						ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
						SignatureMethod = OAuthSignatureMethod.HmacSha1,
						Type = OAuthType.AccessToken
					},
					Method = WebMethod.Get,
					Path = "/" + Globals.PinterestAPIVersion + "/boards/" + args.ObjectID + "/pins/"
				};
				return request;
			};
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the currently authenticated Pinterest User
		/// </summary>
		/// <returns>A Pinterest User object</returns>
		public PinterestUser getUser()
		{
			return getUser("me");
		}

		/// <summary>
		/// Gets the pinterest user with the specified ID value
		/// </summary>
		/// <param name="usernameOrId">The pinterest user ID</param>
		/// <returns>A Pinterest User object</returns>
		public PinterestUser getUser(string usernameOrId)
		{
			PinterestUser user = null;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					Username = usernameOrId
				};

				var request = _userQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					//RaiseException(new Exception(response.Content));
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							user = new PinterestUser()
							{
								ProfileURL = ((Hashtable)table["data"])["profile_url"].ToString(),
								ImageURL = ((Hashtable)table["data"])["image_medium_url"].ToString(),
								FullName = ((Hashtable)table["data"])["full_name"].ToString(),
								Username = ((Hashtable)table["data"])["username"].ToString(),
								Id = ((Hashtable)table["data"])["id"].ToString(),
							};
						}
					}
				}
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					OnPinterestUnauthorized?.Invoke();
				}
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getUser(): " + e.Message);
			}

			return user;
		}

		/// <summary>
		/// Gets a Pin object for the specified Pin ID
		/// </summary>
		/// <param name="pinID">The Pin ID</param>
		/// <returns>A Pin object</returns>
		public Pin getPin(string pinID)
		{
			Pin ret = null;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					ObjectID = pinID
				};

				var request = _pinQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							ret = pinFromHashTable(pinTable);
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getPin(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Get all pins for the currently authenticated user
		/// </summary>
		/// <returns>A List of Pin object</returns>
		public List<Pin> getPins()
		{
			List<Pin> ret = new List<Pin>();

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token
				};

				var request = _pinsQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							ArrayList pinTable = (ArrayList)table["data"];
							for (int x = 0; x < pinTable.Count; x++)
							{
								Hashtable subTable = (Hashtable)pinTable[x];
								ret.Add(pinFromHashTable(subTable));
							}
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getPins(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Gets all pins for the specified Board ID
		/// </summary>
		/// <param name="boardID">The Board ID</param>
		/// <returns>A List of Pin objects</returns>
		public List<Pin> getBoardPins(string boardID)
		{
			List<Pin> ret = new List<Pin>();

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					ObjectID = boardID
				};

				var request = _boardPinsQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							ArrayList pinTable = (ArrayList)table["data"];
							for (int x = 0; x < pinTable.Count; x++)
							{
								Hashtable subTable = (Hashtable)pinTable[x];
								ret.Add(pinFromHashTable(subTable));
							}
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getBoardPins(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Gets a list of boards for the currently authenticated user
		/// </summary>
		/// <returns>A list of board objects</returns>
		public List<Board> getBoardsForUser(string UserID)
		{
			List<Board> ret = new List<Board>();

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					UserID = UserID
				};

				var request = _boardsQuery.Invoke(args);
				request.AddParameter("access_token", _token);
				request.AddParameter("sort", "alphabetical");

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					RaiseMessage("Get Board Content: " + response.Content);
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							ArrayList boardTable = (ArrayList)table["data"];
							for (int x = 0; x < boardTable.Count; x++)
							{
								Hashtable subTable = (Hashtable)boardTable[x];
								ret.Add(boardFromHashTable(subTable));
							}
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getBoards(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Gets a Board object for the specified Board ID
		/// </summary>
		/// <param name="boardID">The Board ID</param>
		/// <returns>A Board object</returns>
		public Board getBoard(string boardID)
		{
			Board ret = null;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					ObjectID = boardID
				};

				var request = _boardQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable boardTable = (Hashtable)table["data"];
							ret = boardFromHashTable(boardTable);
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.getBoard(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Deletes the pin with the specified ID
		/// </summary>
		/// <param name="pinID">The id of the pin to delete</param>
		/// <returns>boolean value</returns>
		public bool deletePin(string pinID)
		{
			bool ret = false;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					ObjectID = pinID
				};

				var request = _deletePinQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
					ret = true;
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.deletePin(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Deletes the board with the specified ID
		/// </summary>
		/// <param name="boardID">The id of the board to delete</param>
		/// <returns>boolean value</returns>
		public bool deleteBoard(string boardID)
		{
			bool ret = false;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token,
					ObjectID = boardID
				};

				var request = _deleteBoardQuery.Invoke(args);
				request.AddParameter("access_token", _token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
					ret = true;
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.deleteBoard(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Creates a board with the specified name and description
		/// </summary>
		/// <param name="name">The name of the board to create</param>
		/// <param name="description">The description of the board to create</param>
		/// <returns>Board object</returns>
		public Board createBoard(string name, string description)
		{
			Board ret = null;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token
				};

				var request = _createBoardQuery.Invoke(args);
				request.AddParameter("access_token", _token);
				request.AddParameter("name", name);
				request.AddParameter("description", description);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.Created)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							ret = boardFromHashTable(pinTable);
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.createBoard(): " + e.Message);
			}

			return ret;
		}

		public Pin createPinUploadImage(string boardID, string note, string link, string fileName, string filePath)
		{
			Pin pin;
			try
			{
				Board b = new Board()
				{
					Id = boardID
				};

				pin = new Pin()
				{
					Board = b,
					Note = note,
					Link = link
				};

				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token
				};

				var request = _createPinQuery.Invoke(args);
				request.AddParameter("access_token", _token);
				request.AddParameter("board_id", pin.Board.Id);
				request.AddParameter("description", pin.Note);
				request.AddParameter("source_url", pin.Link);
				request.AddFile("image", fileName, filePath);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							pin = pinFromHashTable(pinTable);
						}
						else
							pin = null;
					}
					else
						pin = null;
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.createPinBase64Image(): " + e.Message);
				pin = null;
			}

			return pin;
		}

		/// <summary>
		/// Creates a Pinterest Pin
		/// </summary>
		/// <param name="boardID">The boad ID to use</param>
		/// <param name="note">the caption value</param>
		/// <param name="link">the link value</param>
		/// <param name="imageURL">the URL to the image</param>
		/// <returns>A Pin object</returns>
		public Pin createPin(string boardID, string note, string link, string imageURL)
		{
			Pin pin;
			try
			{
				Board b = new Board()
				{
					Id = boardID
				};

				pin = new Pin()
				{
					Board = b,
					Note = note,
					Link = link
				};

				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = _token
				};

				var request = _createPinQuery.Invoke(args);
				request.AddParameter("access_token", _token);
				request.AddParameter("board_id", pin.Board.Id);
				request.AddParameter("description", pin.Note);
				request.AddParameter("source_url", pin.Link);
				request.AddParameter("image_url", imageURL);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							pin = pinFromHashTable(pinTable);
						}
						else
							pin = null;
					}
					else
						pin = null;
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.createPin(): " + e.Message);
				pin = null;
			}

			return pin;
		}
		#endregion

		#region Private Methods
		private void RaiseException(Exception e)
		{
			if (OnPinterestException != null)
			{
				PinterestException ex = new PinterestException(e.Message + "  Stack Trace: " + e.StackTrace);
				OnPinterestException(ex);
			}
		}

		private void RaiseMessage(string Message)
		{
			OnPinterestMessage?.Invoke(Message);
		}

		private Board boardFromHashTable(Hashtable boardTable)
		{
			Board board = null;

			try
			{
				board = new Board()
				{
					URL = boardTable["url"].ToString(),
					Name = boardTable["name"].ToString(),
					Id = boardTable["id"].ToString()
				};
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.boardFromHashTable(): " + e.Message);
			}

			return board;
		}

		private Pin pinFromHashTable(Hashtable pinTable)
		{
			Pin pin = null;

			try
			{
				pin = new Pin()
				{
					URL = pinTable["link"].ToString(),
					Link = pinTable["link"].ToString(),
					Id = pinTable["id"].ToString(),
					Note = pinTable["description"].ToString()
				};
			}
			catch (Exception e)
			{
				RaiseException(e);
				Debug.WriteLine("Exception in PinterestService.pinFromHashTable(): " + e.Message);
			}

			return pin;
		}
		#endregion
	}
}
