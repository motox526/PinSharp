using Hammock;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	[Serializable]
	public class PinterestResponse
	{
		private readonly RestResponseBase _response;
		private readonly Exception _exception;

		internal PinterestResponse(RestResponseBase response, Exception exception = null)
        {
            _exception = exception;
            _response = response;
        }

		private static bool IsStringANumber(IEnumerable<char> limit)
		{
			return limit.All(char.IsNumber);
		}

		public virtual TwitterError Error
		{
			get { return _response.ErrorContentEntity as TwitterError; }
		}

		public virtual NameValueCollection Headers
		{
			get { return _response.Headers; }
		}

		public virtual HttpStatusCode StatusCode
		{
			get { return _response.StatusCode; }
			set { _response.StatusCode = value; }
		}

		public virtual bool SkippedDueToRateLimitingRule
		{
			get { return _response.SkippedDueToRateLimitingRule; }
			set { _response.SkippedDueToRateLimitingRule = value; }
		}

		public virtual string StatusDescription
		{
			get { return _response.StatusDescription; }
			set { _response.StatusDescription = value; }
		}

		public virtual string Response
		{
			get { return _response.Content; }
		}

		public virtual string RequestMethod
		{
			get { return _response.RequestMethod; }
			set { _response.RequestMethod = value; }
		}

		public virtual Uri RequestUri
		{
			get { return _response.RequestUri; }
			set { _response.RequestUri = value; }
		}

		public virtual DateTime? ResponseDate
		{
			get { return _response.ResponseDate; }
			set { _response.ResponseDate = value; }
		}

		public virtual DateTime? RequestDate
		{
			get { return _response.RequestDate; }
			set { _response.RequestDate = value; }
		}

		public virtual Exception InnerException
		{
			get { return _exception ?? _response.InnerException; }
		}
	}
}
