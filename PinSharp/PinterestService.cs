using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

/*Copyright © 2016, Chris Butterfield Software Solutions, LLC
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
				GetErrorResponseEntityType = (request, @base) => typeof(TwitterError),
			};
		}
		#endregion

		#region Getters/Setters
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
					Path = "/v1/" + args.Username + "/"
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
					Path = "/v1/me/pins/" + (String.IsNullOrEmpty(args.ObjectID) ? "" : (args.ObjectID + "/"))
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
					Path = "/v1/me/boards/"
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
					Path = "/v1/boards/"
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
					Path = "/v1/pins/" + args.ObjectID + "/"
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
					Method = WebMethod.Post,
					Path = "/v1/pins/"
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
					Path = "/v1/pins/" + args.ObjectID + "/"
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
					Path = "/v1/boards/" + args.ObjectID + "/"
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
					Path = "/v1/boards/" + args.ObjectID + "/"
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
					Path = "/v1/boards/" + args.ObjectID + "/pins/" 
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
					Token = this._token,
					Username = usernameOrId
				};

				var request = _userQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							user = new PinterestUser()
							{
								URL = ((Hashtable)table["data"])["url"].ToString(),
								FirstName = ((Hashtable)table["data"])["first_name"].ToString(),
								LastName = ((Hashtable)table["data"])["last_name"].ToString(),
								Id = ((Hashtable)table["data"])["id"].ToString(),
							};
						}
					}
				}
			}
			catch (Exception e)
			{
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
					Token = this._token,
					ObjectID = pinID
				};

				var request = _pinQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							ret = this.pinFromHashTable(pinTable);
						}
					}
				}
			}
			catch (Exception e)
			{
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
					Token = this._token
				};

				var request = _pinsQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
								ret.Add(this.pinFromHashTable(subTable));
							}
						}
					}
				}
			}
			catch (Exception e)
			{
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
					Token = this._token,
					ObjectID = boardID
				};

				var request = _boardPinsQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
								ret.Add(this.pinFromHashTable(subTable));
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in PinterestService.getBoardPins(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Gets a list of boards for the currently authenticated user
		/// </summary>
		/// <returns>A list of board objects</returns>
		public List<Board> getBoards()
		{
			List<Board> ret = new List<Board>();

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = this._token
				};

				var request = _boardsQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
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
								ret.Add(this.boardFromHashTable(subTable));
							}
						}
					}
				}
			}
			catch (Exception e)
			{
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
					Token = this._token,
					ObjectID = boardID
				};

				var request = _boardQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable boardTable = (Hashtable)table["data"];
							ret = this.boardFromHashTable(boardTable);
						}
					}
				}
			}
			catch (Exception e)
			{
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
					Token = this._token,
					ObjectID = pinID
				};

				var request = _deletePinQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					ret = true;
			}
			catch (Exception e)
			{
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
					Token = this._token,
					ObjectID = boardID
				};

				var request = _deleteBoardQuery.Invoke(args);
				request.AddParameter("access_token", this._token);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					ret = true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in PinterestService.deleteBoard(): " + e.Message);
			}

			return ret;
		}

		public Board createBoard(string name, string description)
		{
			Board ret = null;

			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = this._token
				};

				var request = _createBoardQuery.Invoke(args);
				request.AddParameter("access_token", this._token);
				request.AddParameter("name", name);
				request.AddParameter("description", description);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.Created)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							ret = this.boardFromHashTable(pinTable);
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in PinterestService.createBoard(): " + e.Message);
			}

			return ret;
		}

        public Pin createPinUploadImage(string boardID, string note, string link, string fileName, string filePath)
        {
            Pin pin = null;

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
                    Token = this._token
                };

                var request = _createPinQuery.Invoke(args);
                request.AddParameter("access_token", this._token);
                request.AddParameter("board", pin.Board.Id);
                request.AddParameter("note", pin.Note);
                request.AddParameter("link", pin.Link);
                request.AddFile("image", fileName, filePath);

                var response = _oauth.Request(request);

                SetResponse(response);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var query = JSON.JsonDecode(response.Content);
                    if ((query != null) && (query is Hashtable))
                    {
                        Hashtable table = (Hashtable)query;
                        if (table.ContainsKey("data"))
                        {
                            Hashtable pinTable = (Hashtable)table["data"];
                            pin = this.pinFromHashTable(pinTable);
                        }
                        else
                            pin = null;
                    }
                    else
                        pin = null;
                }
                else
                    pin = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in PinterestService.createPinBase64Image(): " + e.Message);
                pin = null;
            }

            return pin;
        }

		public Pin createPin(string boardID, string note, string link, string imageURL)
		{
			Pin pin = null;

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
					Token = this._token
				};

				var request = _createPinQuery.Invoke(args);
				request.AddParameter("access_token", this._token);
				request.AddParameter("board", pin.Board.Id);
				request.AddParameter("note", pin.Note);
				request.AddParameter("link", pin.Link);
				request.AddParameter("image_url", imageURL);

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.Created)
				{
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("data"))
						{
							Hashtable pinTable = (Hashtable)table["data"];
							pin = this.pinFromHashTable(pinTable);
						}
						else
							pin = null;
					}
					else
						pin = null;
				}
				else
					pin = null;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in PinterestService.createPin(): " + e.Message);
				pin = null;
			}

			return pin;
		}
		#endregion

		#region Private Methods
		private Board boardFromHashTable(Hashtable pinTable)
		{
			Board board = null;

			try
			{
				board = new Board()
				{
					URL = pinTable["url"].ToString(),
					Name = pinTable["name"].ToString(),
					Id = pinTable["id"].ToString()
				};
			}
			catch (Exception e)
			{
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
					URL = pinTable["url"].ToString(),
					Link = pinTable["link"].ToString(),
					Id = pinTable["id"].ToString(),
					Note = pinTable["note"].ToString()
				};
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in PinterestService.pinFromHashTable(): " + e.Message);
			}

			return pin;
		}
		#endregion
	}
}
