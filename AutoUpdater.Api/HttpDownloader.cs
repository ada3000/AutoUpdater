using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography;

using iTeco.Lib.Base;
using iTeco.Lib.Srv;


namespace AutoUpdater.Api
{
	/// <summary>
	/// Загрузка данных
	/// </summary>
	public static class HttpDownloader
	{
		public static string Get(string url, Stream destStream = null)
		{
			HttpWebRequest wr = Create(url);

			HttpWebResponse resp = wr.GetResponse() as HttpWebResponse;

			using (var source = resp.GetResponseStream())
			{
				if (destStream != null)
				{
					source.CopyTo(destStream);
					return null;
				}
				else
					return ReadResponce(source);
			}
		}
		private static string ReadResponce(Stream stream)
		{
			string defaultCharSet = "UTF-8";
			byte[] buff = new byte[0x2000];

			using (MemoryStream ms = new MemoryStream())
			{
				int cnt = 0;
				while ((cnt = stream.Read(buff, 0, buff.Length)) > 0)
					ms.Write(buff, 0, cnt);

				ms.Position = 0;
				buff = new byte[ms.Length];
				ms.Read(buff, 0, buff.Length);
			}
			string content = System.Text.Encoding.GetEncoding(defaultCharSet).GetString(buff);

			return content;
		}

		public static string Post(string url, string postData)
		{
			HttpWebRequest wr = Create(url, "POST");

			using(var postStream = wr.GetRequestStream())
			{
				byte[] buff = System.Text.Encoding.UTF8.GetBytes(postData);
				postStream.Write(buff, 0, buff.Length);
			}
			HttpWebResponse resp = wr.GetResponse() as HttpWebResponse;

			using (var source = resp.GetResponseStream())
				return ReadResponce(source);
		}

		/// <summary>
		/// Создание запроса
		/// </summary>
		/// <param name="headers"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		private static HttpWebRequest Create(string obj, string method = "GET")
		{
			/*
			Mozilla/5.0 (Windows NT 6.1; rv:8.0) Gecko/20100101 Firefox/8.0
			Accept-Language	ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3
			Accept-Encoding	deflate
			Accept-Charset	windows-1251,utf-8;q=0.7,*;q=0.7
			*/

			HttpWebRequest wr = HttpWebRequest.Create(obj) as HttpWebRequest;
			wr.AllowAutoRedirect = false;
			wr.Method = method;
			wr.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:8.0) Gecko/20100101 Firefox/8.0";
			wr.Headers.Add("Accept-Language", "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3");
			wr.Headers.Add("Accept-Charset", "windows-1251,utf-8;q=0.7,*;q=0.7");
			//wr.Headers.Add("Accept-Encoding", "deflate");

			return wr;
		}
	}
}
