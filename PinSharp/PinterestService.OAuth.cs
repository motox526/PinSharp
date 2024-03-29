﻿using Hammock;
using Hammock.Web;
using System;
using System.Collections;

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
		private readonly RestClient _oauth;

		[Serializable]
		private class FunctionArguments
		{
			public string AppID { get; set; }
			public string AppSecret { get; set; }
			public string Token { get; set; }
			public string TokenSecret { get; set; }
			public string Verifier { get; set; }
			public string Username { get; set; }
			public string Password { get; set; }
			public string ObjectID { get; set; }
			public string UserID { get; set; }
		}

		//private readonly Func<FunctionArguments, RestRequest> _requestTokenQuery
		//	= args =>
		//	{
		//		var request = new RestRequest
		//		{
		//			Credentials = new OAuthCredentials
		//			{
		//				ConsumerKey = args.AppID,
		//				ConsumerSecret = args.AppSecret,
		//				ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
		//				SignatureMethod = OAuthSignatureMethod.HmacSha1,
		//				Type = OAuthType.RequestToken
		//			},
		//			Method = WebMethod.Get,
		//			Path = "/oauth"
		//		};
		//		return request;
		//	};

		private readonly Func<FunctionArguments, RestRequest> _accessTokenQuery
			= args =>
			{
				var request = new RestRequest
				{
					//Credentials = new OAuthCredentials
					//{
					//	ConsumerKey = args.AppID,
					//	ConsumerSecret = args.AppSecret,
					//	Token = args.Token,
					//	Verifier = args.Verifier,
					//	ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
					//	SignatureMethod = OAuthSignatureMethod.HmacSha1,
					//	Type = OAuthType.AccessToken
					//},
					Method = WebMethod.Post,
					Path = "/" + Globals.PinterestAPIVersion + "/oauth/access_token/"
				};
				return request;
			};

		public string GetAuthorizationURL()
		{
			return Globals.PinterestAuthorizationURL + "/oauth/";
		}

		public string getAuthorizationRequestURL(string callback, string scope)
		{
			state = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
			string ret = GetAuthorizationURL() + "?response_type=code&client_id=" + _appID
				+ "&client_secret=" + _appSecret
				+ "&state=" + state
				//+ "&scope=" + scope 
				+ "&redirect_uri=" + callback;

			return ret;
		}

		//public string getAccessTokenRequestURL(string oauth_token, string callback)
		//{
		//	string ret = Globals.PinterestBaseURL + "/" + Globals.PinterestAPIVersion + "/oauth/access_token" + "?code=" + oauth_token
		//		+ "&grant_type=authorization_code"
		//		+ "&redirect_uri=" + callback;

		//	return ret;
		//}

		/// <summary>
		/// Gets the state value
		/// </summary>
		public string state { get; private set; } = "";

		//public virtual OAuthRequestToken GetRequestToken(string callback, string scope)
		//{
		//	var args = new FunctionArguments
		//	{
		//		AppID = _appID,
		//		AppSecret = _appSecret
		//	};

		//	var request = _requestTokenQuery.Invoke(args);
		//	if (!callback.IsNullOrBlank())
		//	{
		//		state = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
		//		request.AddParameter("response_type", "token");
		//		request.AddParameter("redirect_uri", callback);
		//		request.AddParameter("scope", scope);
		//		request.AddParameter("state", state);
		//	}

		//	var response = _oauth.Request(request);

		//	SetResponse(response);

		//	var query = HttpUtility.ParseQueryString(response.Content);
		//	var oauth = new OAuthRequestToken
		//	{
		//		Token = query["code"] ?? "?",
		//		OAuthCallbackConfirmed = !string.IsNullOrEmpty(query["code"] ?? "")
		//	};

		//	return oauth;
		//}

		public virtual OAuthAccessToken GetAccessToken(string code, string verifier, string Callback)
		{
			OAuthRequestToken requestToken = new OAuthRequestToken() { Token = code };
			return GetAccessToken(requestToken, verifier, Callback);
		}

		public virtual OAuthAccessToken GetAccessToken(OAuthRequestToken requestToken, string verifier, string Callback)
		{
			try
			{
				var args = new FunctionArguments
				{
					AppID = _appID,
					AppSecret = _appSecret,
					Token = requestToken.Token,
					Verifier = verifier
				};

				var request = _accessTokenQuery.Invoke(args);

				request.AddParameter("grant_type", "authorization_code");
				request.AddParameter("code", requestToken.Token);
				request.AddParameter("redirect_uri", Callback);
				request.AddParameter("client_id", _appID);
				request.AddParameter("client_secret", _appSecret);

				//RaiseException(new Exception("Request URL: " + request.Path));

				var response = _oauth.Request(request);

				SetResponse(response);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//RaiseException(new Exception("Response: " + response.Content));
					var query = JSON.JsonDecode(response.Content);
					if ((query != null) && (query is Hashtable))
					{
						Hashtable table = (Hashtable)query;
						if (table.ContainsKey("access_token"))
						{
							var accessToken = new OAuthAccessToken
							{
								Token = table["access_token"].ToString() ?? "?"//,
																			   //UserId = Convert.ToInt64(query["user_id"] ?? "0")
							};

							_token = accessToken.Token;
							return accessToken;
						}
					}
				}
				else
					throw new Exception("Received invalid response from Pinterest.  Status code: " + response.StatusCode + " Content: " + response.Content);

				return null;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Exception in PinterestService.GetAccessToken(): " + e.Message);
				RaiseException(e);
			}

			return null;
		}

		private void SetResponse(RestResponseBase response)
		{
			Response = new PinterestResponse(response);
		}
	}
}
