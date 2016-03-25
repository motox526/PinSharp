using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

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
	/// <summary>
	/// This class encodes and decodes JSON strings.
	/// Spec. details, see http://www.json.org/
	///
	/// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
	/// All numbers are parsed to doubles.
	/// </summary>
	public class JSON
	{
		private const int TOKEN_NONE = 0;
		private const int TOKEN_CURLY_OPEN = 1;
		private const int TOKEN_CURLY_CLOSE = 2;
		private const int TOKEN_SQUARED_OPEN = 3;
		private const int TOKEN_SQUARED_CLOSE = 4;
		private const int TOKEN_COLON = 5;
		private const int TOKEN_COMMA = 6;
		private const int TOKEN_STRING = 7;
		private const int TOKEN_NUMBER = 8;
		private const int TOKEN_TRUE = 9;
		private const int TOKEN_FALSE = 10;
		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		/// <summary>
		/// The valid web request methods
		/// </summary>
		public enum Request_Methods
		{
			/// <summary>
			/// GET method
			/// </summary>
			GET,
			/// <summary>
			/// PUT method
			/// </summary>
			PUT,
			/// <summary>
			/// POST method
			/// </summary>
			POST,
			/// <summary>
			/// DELETE method
			/// </summary>
			DELETE
		}

		/// <summary>
		/// Executes a DELETE request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_DELETE_Request(string URL)
		{
			return Do_DELETE_Request(URL, null);
		}

		/// <summary>
		/// Executes a DELETE request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <param name="headers">A list of HTTP Headers</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_DELETE_Request(string URL, Dictionary<string, string> headers)
		{
			return Make_JSON_Web_Request(URL, headers, Request_Methods.DELETE);
		}

		/// <summary>
		/// Executes a POST request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <param name="JSONData">The JSON formated data object</param>
		/// <param name="contentType">The Content type</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_POST_Request(string URL, string JSONData, string contentType)
		{
			return Do_POST_Request(URL, JSONData, contentType, null);
		}

		/// <summary>
		/// Executes a POST request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <param name="JSONData">The JSON formated data object</param>
		/// <param name="contentType">The Content type</param>
		/// <param name="headers">A list of HTTP Headers</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_POST_Request(
			string URL,
			string JSONData,
			string contentType,
			Dictionary<string, string> headers)
		{
			return Do_PUT_POST_Request(URL, JSONData, contentType, headers, Request_Methods.POST);
		}

		/// <summary>
		/// Executes a PUT request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <param name="JSONData">The JSON formated data object</param>
		/// <param name="contentType">The Content type</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_PUT_Request(string URL, string JSONData, string contentType)
		{
			return Do_PUT_Request(URL, JSONData, contentType, null);
		}

		/// <summary>
		/// Executes a PUT request against the specified URL, and writes the Specified JSON data object to the server
		/// </summary>
		/// <param name="URL">The URL</param>
		/// <param name="JSONData">The JSON formated data object</param>
		/// <param name="contentType">The Content type</param>
		/// <param name="headers">A list of HTTP Headers</param>
		/// <returns>the resulting JSON data object.</returns>
		public static object Do_PUT_Request(
			string URL,
			string JSONData,
			string contentType,
			Dictionary<string, string> headers)
		{
			return Do_PUT_POST_Request(URL, JSONData, contentType, headers, Request_Methods.PUT);
		}

		/// <summary>
		/// Makes a GET request to the specified URL and writes the response data out to the specfied file
		/// </summary>
		/// <param name="webRequestURL">The URL to make the web request to</param>
		/// <param name="headers">Any HttpRequestHeader values to add to the request</param>
		/// <param name="FilePath">The path to the file to write the response data to</param>
		/// <returns>Boolean value</returns>
		public static bool Get_Binary_File_From_WebRequest(
			string webRequestURL,
			Dictionary<string, string> headers,
			string FilePath)
		{
			bool Success = false;

			FileStream fileStream = null;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webRequestURL);

				if ((headers != null) && (headers.Count > 0))
					Set_Headers(ref request, headers);

				fileStream = File.Create(FilePath);
				using (Stream objStream = request.GetResponse().GetResponseStream())
				{
					objStream.CopyTo(fileStream);
				}

				request = null;
				Success = true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in JSON.Get_Binary_File_From_WebRequest(): " + e.Message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream.Dispose();
					fileStream = null;
				}
			}

			return Success;
		}

		/// <summary>
		/// Makes a GET request to the specified URL and writes the response data out to an Image object
		/// </summary>
		/// <param name="webRequestURL">The URL to make the web request to</param>
		/// <param name="headers">Any HttpRequestHeader values to add to the request</param>
		/// <returns>An Image object</returns>
		public static Image Get_Image_From_WebRequest(string webRequestURL, Dictionary<string, string> headers)
		{
			Image ret = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webRequestURL);

				if ((headers != null) && (headers.Count > 0))
					Set_Headers(ref request, headers);

				//Don't use StreamReader here, as it messes up the formatting
				using (Stream objStream = request.GetResponse().GetResponseStream())
				{
					ret = Image.FromStream(objStream);
				}

				request = null;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in JSON.Get_Image_From_WebRequest(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Creates and makes a WEB request to the specified URL, and returns the decoded JSON data in the form of a HashTable
		/// </summary>
		/// <param name="webRequestURL">The URL to make the web request to</param>
		/// <param name="headers">Any HttpRequestHeader values to add to the request</param>
		/// <param name="Method">The Request Method</param>
		/// <returns>A HashTable object</returns>
		public static object Make_JSON_Web_Request(string webRequestURL, Dictionary<string, string> headers, Request_Methods Method)
		{
			object ret = null;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webRequestURL);

				if ((headers != null) && (headers.Count > 0))
					Set_Headers(ref request, headers);
				//request.ContentType = "application/x-www-form-urlencoded";
				request.Method = Method.ToString();
				Stream objStream = request.GetResponse().GetResponseStream();
				StreamReader objReader = new StreamReader(objStream);

				string sLine = "";
				string response = "";
				int i = 0;

				while (sLine != null)
				{
					i++;
					sLine = objReader.ReadLine();
					if (sLine != null)
					{
						response += sLine;
						//Debug.WriteLine("{0}:{1}", i, sLine);
					}
				}

				request = null;
				objReader.Dispose();
				objReader = null;
				objStream.Close();
				objStream.Dispose();
				objStream = null;

				if (!String.IsNullOrEmpty(response))
					ret = JSON.JsonDecode(response);
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in JSON.Make_JSON_Web_Request(): " + e.Message);
			}

			return ret;
		}

		/// <summary>
		/// Creates and makes a WEB request to the specified URL, and returns the decoded JSON data in the form of a HashTable
		/// </summary>
		/// <param name="webRequestURL">The URL to make the web request to</param>
		/// <param name="headers">Any HttpRequestHeader values to add to the request</param>
		/// <returns>A HashTable object</returns>
		public static object Make_JSON_Web_Request(string webRequestURL, Dictionary<string, string> headers)
		{
			return Make_JSON_Web_Request(webRequestURL, headers, Request_Methods.GET);
		}

		/// <summary>
		/// Creates and makes a WEB request to the specified URL, and returns the decoded JSON data in the form of a HashTable
		/// </summary>
		/// <param name="webRequestURL">The URL to make the web request to</param>
		/// <returns>A HashTable object</returns>
		public static object Make_JSON_Web_Request(string webRequestURL)
		{
			return Make_JSON_Web_Request(webRequestURL, null);
		}

		private static object Do_PUT_POST_Request(
			string URL,
			string JSONData,
			string contentType,
			Dictionary<string, string> headers,
			Request_Methods Method)
		{
			object ret = null;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
				if (!String.IsNullOrEmpty(contentType))
					httpWebRequest.ContentType = contentType;
				httpWebRequest.Method = Method.ToString();

				if ((headers != null) && (headers.Count > 0))
					Set_Headers(ref httpWebRequest, headers);

				byte[] byteArray = Encoding.UTF8.GetBytes(JSONData);
				httpWebRequest.ContentLength = byteArray.Length;

				using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					streamWriter.Write(JSONData);
				}

				HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var responseText = streamReader.ReadToEnd();
					//Now you have your response.
					//or false depending on information in the response
					if (!String.IsNullOrEmpty(responseText))
						ret = JSON.JsonDecode(responseText);
				}

				httpResponse.Close();
				httpResponse = null;
				httpWebRequest = null;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in JSON.Do_PUT_POST_Request(): " + e.Message);
				ret = e.Message;
			}

			return ret;
		}

		private static void Set_Headers(ref HttpWebRequest request, Dictionary<string, string> headers)
		{
			try
			{
				if ((headers != null) && (headers.Count > 0))
				{
					foreach (string key in headers.Keys)
					{
						string value = headers[key];
						HttpRequestHeader headerenumvalue;
						if (Enum.TryParse<HttpRequestHeader>(key, true, out headerenumvalue))
						{
							switch (headerenumvalue)
							{
								case HttpRequestHeader.Accept:
									request.Accept = value;
									break;
								default:
									request.Headers.Add(headerenumvalue, value);
									break;
							}
						}
						else
							request.Headers.Add(key, value);
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception in JSON.Set_Headers(): " + e.Message);
			}
		}

		/// <summary>
		/// Parses the string json into a value
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
		public static object JsonDecode(string json)
		{
			bool success = true;
			return JsonDecode(json, ref success);
		}

		/// <summary>
		/// Parses the string json into a value; and fills 'success' with the successfullness of the parse.
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <param name="success">Successful parse?</param>
		/// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
		public static object JsonDecode(string json, ref bool success)
		{
			success = true;
			if (json != null)
			{
				char[] charArray = json.ToCharArray();
				int index = 0;
				object value = ParseValue(charArray, ref index, ref success);
				if (value != null)
					return value;
				else
					return json;
			}
			else
				return null;
		}

		/// <summary>
		/// Converts a Hashtable / ArrayList object into a JSON string
		/// </summary>
		/// <param name="json">A Hashtable / ArrayList</param>
		/// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
		public static string JsonEncode(object json)
		{
			StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
			bool success = SerializeValue(json, builder);
			return (success ? builder.ToString() : null);
		}

		private static Hashtable ParseObject(char[] json, ref int index, ref bool success)
		{
			Hashtable table = new Hashtable();
			int token;

			// {
			NextToken(json, ref index);

			bool done = false;
			while (!done)
			{
				token = LookAhead(json, index);
				if (token == JSON.TOKEN_NONE)
				{
					success = false;
					return null;
				}
				else if (token == JSON.TOKEN_COMMA)
					NextToken(json, ref index);
				else if (token == JSON.TOKEN_CURLY_CLOSE)
				{
					NextToken(json, ref index);
					return table;
				}
				else
				{
					// name
					string name = ParseString(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}

					// :
					token = NextToken(json, ref index);
					if (token != JSON.TOKEN_COLON)
					{
						success = false;
						return null;
					}

					// value
					object value = ParseValue(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}

					table[name] = value;
				}
			}

			return table;
		}

		private static ArrayList ParseArray(char[] json, ref int index, ref bool success)
		{
			ArrayList array = new ArrayList();

			// [
			NextToken(json, ref index);

			bool done = false;
			while (!done)
			{
				int token = LookAhead(json, index);
				if (token == JSON.TOKEN_NONE)
				{
					success = false;
					return null;
				}
				else if (token == JSON.TOKEN_COMMA)
					NextToken(json, ref index);
				else if (token == JSON.TOKEN_SQUARED_CLOSE)
				{
					NextToken(json, ref index);
					break;
				}
				else
				{
					object value = ParseValue(json, ref index, ref success);
					if (!success)
						return null;

					array.Add(value);
				}
			}

			return array;
		}

		private static object ParseValue(char[] json, ref int index, ref bool success)
		{
			switch (LookAhead(json, index))
			{
				case JSON.TOKEN_STRING:
					return ParseString(json, ref index, ref success);
				case JSON.TOKEN_NUMBER:
					return ParseNumber(json, ref index, ref success);
				case JSON.TOKEN_CURLY_OPEN:
					return ParseObject(json, ref index, ref success);
				case JSON.TOKEN_SQUARED_OPEN:
					return ParseArray(json, ref index, ref success);
				case JSON.TOKEN_TRUE:
					NextToken(json, ref index);
					return true;
				case JSON.TOKEN_FALSE:
					NextToken(json, ref index);
					return false;
				case JSON.TOKEN_NULL:
					NextToken(json, ref index);
					return null;
				case JSON.TOKEN_NONE:
					break;
			}

			success = false;
			return null;
		}

		private static string ParseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder s = new StringBuilder(BUILDER_CAPACITY);
			char c;

			EatWhitespace(json, ref index);

			// "
			c = json[index++];

			bool complete = false;
			while (!complete)
			{
				if (index == json.Length)
					break;

				c = json[index++];
				if (c == '"')
				{
					complete = true;
					break;
				}
				else if (c == '\\')
				{
					if (index == json.Length)
						break;
					c = json[index++];
					if (c == '"')
						s.Append('"');
					else if (c == '\\')
						s.Append('\\');
					else if (c == '/')
						s.Append('/');
					else if (c == 'b')
						s.Append('\b');
					else if (c == 'f')
						s.Append('\f');
					else if (c == 'n')
						s.Append('\n');
					else if (c == 'r')
						s.Append('\r');
					else if (c == 't')
						s.Append('\t');
					else if (c == 'u')
					{
						int remainingLength = json.Length - index;
						if (remainingLength >= 4)
						{
							// parse the 32 bit hex into an integer codepoint
							uint codePoint;
							if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out codePoint)))
								return "";
							// convert the integer codepoint to a unicode char and add to string
							s.Append(Char.ConvertFromUtf32((int)codePoint));
							// skip 4 chars
							index += 4;
						}
						else
							break;
					}
				}
				else
					s.Append(c);
			}

			if (!complete)
			{
				success = false;
				return null;
			}

			return s.ToString();
		}

		private static double ParseNumber(char[] json, ref int index, ref bool success)
		{
			EatWhitespace(json, ref index);

			int lastIndex = GetLastIndexOfNumber(json, index);
			int charLength = (lastIndex - index) + 1;

			double number;
			success = Double.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);

			index = lastIndex + 1;
			return number;
		}

		private static int GetLastIndexOfNumber(char[] json, int index)
		{
			int lastIndex;

			for (lastIndex = index; lastIndex < json.Length; lastIndex++)
			{
				if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
					break;
			}
			return lastIndex - 1;
		}

		private static void EatWhitespace(char[] json, ref int index)
		{
			for (; index < json.Length; index++)
			{
				if (" \t\n\r".IndexOf(json[index]) == -1)
					break;
			}
		}

		private static int LookAhead(char[] json, int index)
		{
			int saveIndex = index;
			return NextToken(json, ref saveIndex);
		}

		private static int NextToken(char[] json, ref int index)
		{
			EatWhitespace(json, ref index);

			if (index == json.Length)
				return JSON.TOKEN_NONE;

			char c = json[index];
			index++;
			switch (c)
			{
				case '{':
					return JSON.TOKEN_CURLY_OPEN;
				case '}':
					return JSON.TOKEN_CURLY_CLOSE;
				case '[':
					return JSON.TOKEN_SQUARED_OPEN;
				case ']':
					return JSON.TOKEN_SQUARED_CLOSE;
				case ',':
					return JSON.TOKEN_COMMA;
				case '"':
					return JSON.TOKEN_STRING;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '-':
					return JSON.TOKEN_NUMBER;
				case ':':
					return JSON.TOKEN_COLON;
			}
			index--;

			int remainingLength = json.Length - index;

			// false
			if (remainingLength >= 5)
			{
				if (json[index] == 'f' &&
					json[index + 1] == 'a' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 's' &&
					json[index + 4] == 'e')
				{
					index += 5;
					return JSON.TOKEN_FALSE;
				}
			}

			// true
			if (remainingLength >= 4)
			{
				if (json[index] == 't' &&
					json[index + 1] == 'r' &&
					json[index + 2] == 'u' &&
					json[index + 3] == 'e')
				{
					index += 4;
					return JSON.TOKEN_TRUE;
				}
			}

			// null
			if (remainingLength >= 4)
			{
				if (json[index] == 'n' &&
					json[index + 1] == 'u' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 'l')
				{
					index += 4;
					return JSON.TOKEN_NULL;
				}
			}

			return JSON.TOKEN_NONE;
		}

		/// <summary>
		/// Serializes an object to a string and adds it to the string builder object
		/// </summary>
		/// <param name="value">The object to serialize</param>
		/// <param name="builder">The StringBuilder object</param>
		/// <returns>Boolean value</returns>
		private static bool SerializeValue(object value, StringBuilder builder)
		{
			bool success = true;

			if (value is string)
				success = SerializeString((string)value, builder);
			else if (value is Hashtable)
				success = SerializeObject((Hashtable)value, builder);
			else if (value is ArrayList)
				success = SerializeArray((ArrayList)value, builder);
			else if ((value is Boolean) && ((Boolean)value == true))
				builder.Append("true");
			else if ((value is Boolean) && ((Boolean)value == false))
				builder.Append("false");
			else if (value is ValueType)
			{
				// thanks to ritchie for pointing out ValueType to me
				success = SerializeNumber(Convert.ToDouble(value), builder);
			}
			else if (value == null)
				builder.Append("null");
			else
				success = false;
			return success;
		}

		/// <summary>
		/// Serializes a HashTable to a string and adds it to the string builder object
		/// </summary>
		/// <param name="anObject">The HashTable to serialize</param>
		/// <param name="builder">The StringBuilder object</param>
		/// <returns>Boolean value</returns>
		private static bool SerializeObject(Hashtable anObject, StringBuilder builder)
		{
			builder.Append("{");

			IDictionaryEnumerator e = anObject.GetEnumerator();
			bool first = true;
			while (e.MoveNext())
			{
				string key = e.Key.ToString();
				object value = e.Value;

				if (!first)
					builder.Append(", ");

				SerializeString(key, builder);
				builder.Append(":");
				if (!SerializeValue(value, builder))
					return false;

				first = false;
			}

			builder.Append("}");
			return true;
		}

		/// <summary>
		/// Serializes an Array to a string and adds it to the string builder object
		/// </summary>
		/// <param name="anArray">The Array to serialize</param>
		/// <param name="builder">The StringBuilder object</param>
		/// <returns>Boolean value</returns>
		private static bool SerializeArray(ArrayList anArray, StringBuilder builder)
		{
			builder.Append("[");

			bool first = true;
			for (int i = 0; i < anArray.Count; i++)
			{
				object value = anArray[i];

				if (!first)
					builder.Append(", ");

				if (!SerializeValue(value, builder))
					return false;

				first = false;
			}

			builder.Append("]");
			return true;
		}

		/// <summary>
		/// Serializes a string and adds it to the string builder object
		/// </summary>
		/// <param name="aString">The string to serialize</param>
		/// <param name="builder">The StringBuilder object</param>
		/// <returns>Boolean value</returns>
		private static bool SerializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");

			char[] charArray = aString.ToCharArray();
			for (int i = 0; i < charArray.Length; i++)
			{
				char c = charArray[i];
				if (c == '"')
					builder.Append("\\\"");
				else if (c == '\\')
					builder.Append("\\\\");
				else if (c == '\b')
					builder.Append("\\b");
				else if (c == '\f')
					builder.Append("\\f");
				else if (c == '\n')
					builder.Append("\\n");
				else if (c == '\r')
					builder.Append("\\r");
				else if (c == '\t')
					builder.Append("\\t");
				else
				{
					int codepoint = Convert.ToInt32(c);
					if ((codepoint >= 32) && (codepoint <= 126))
						builder.Append(c);
					else
						builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
				}
			}

			builder.Append("\"");
			return true;
		}

		/// <summary>
		/// Serializes a number to a string and adds it to the string builder object
		/// </summary>
		/// <param name="number">The number to serialize</param>
		/// <param name="builder">The StringBuilder object</param>
		/// <returns>Boolean value</returns>
		private static bool SerializeNumber(double number, StringBuilder builder)
		{
			builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
			return true;
		}
	}

	/// <summary>
	/// The valid JwtHashAlgorithm value
	/// </summary>
	public enum JwtHashAlgorithm
	{
		/// <summary>
		/// RS256
		/// </summary>
		RS256,
		/// <summary>
		/// HS384
		/// </summary>
		HS384,
		/// <summary>
		/// HS512
		/// </summary>
		HS512
	}

	public class JsonWebToken
	{
		private readonly static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

		/// <summary>
		/// Static constructor
		/// </summary>
		static JsonWebToken()
		{
			HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                { JwtHashAlgorithm.RS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };
		}

		public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
		{
			return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
		}

		public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
		{
			var segments = new List<string>();
			var header = new { alg = algorithm.ToString(), typ = "JWT" };

			byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
			byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));
			//byte[] payloadBytes = Encoding.UTF8.GetBytes(@"{"iss":"761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com","scope":"https://www.googleapis.com/auth/prediction","aud":"https://accounts.google.com/o/oauth2/token","exp":1328554385,"iat":1328550785}");

			segments.Add(Base64UrlEncode(headerBytes));
			segments.Add(Base64UrlEncode(payloadBytes));

			var stringToSign = string.Join(".", segments.ToArray());

			var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

			byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
			segments.Add(Base64UrlEncode(signature));

			return string.Join(".", segments.ToArray());
		}

		public static string Decode(string token, string key)
		{
			return Decode(token, key, true);
		}

		public static string Decode(string token, string key, bool verify)
		{
			var parts = token.Split('.');
			var header = parts[0];
			var payload = parts[1];
			byte[] crypto = Base64UrlDecode(parts[2]);

			var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
			var headerData = JObject.Parse(headerJson);
			var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
			var payloadData = JObject.Parse(payloadJson);

			if (verify)
			{
				var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
				var keyBytes = Encoding.UTF8.GetBytes(key);
				var algorithm = (string)headerData["alg"];

				var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
				var decodedCrypto = Convert.ToBase64String(crypto);
				var decodedSignature = Convert.ToBase64String(signature);

				if (decodedCrypto != decodedSignature)
				{
					throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
				}
			}

			return payloadData.ToString();
		}

		private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
		{
			switch (algorithm)
			{
				case "RS256": return JwtHashAlgorithm.RS256;
				case "HS384": return JwtHashAlgorithm.HS384;
				case "HS512": return JwtHashAlgorithm.HS512;
				default: throw new InvalidOperationException("Algorithm not supported.");
			}
		}

		// from JWT spec
		private static string Base64UrlEncode(byte[] input)
		{
			var output = Convert.ToBase64String(input);
			output = output.Split('=')[0]; // Remove any trailing '='s
			output = output.Replace('+', '-'); // 62nd char of encoding
			output = output.Replace('/', '_'); // 63rd char of encoding
			return output;
		}

		// from JWT spec
		private static byte[] Base64UrlDecode(string input)
		{
			var output = input;
			output = output.Replace('-', '+'); // 62nd char of encoding
			output = output.Replace('_', '/'); // 63rd char of encoding
			switch (output.Length % 4) // Pad with trailing '='s
			{
				case 0: break; // No pad chars in this case
				case 2: output += "=="; break; // Two pad chars
				case 3: output += "="; break; // One pad char
				default: throw new System.Exception("Illegal base64url string!");
			}
			var converted = Convert.FromBase64String(output); // Standard base64 decoder
			return converted;
		}
	}
}
