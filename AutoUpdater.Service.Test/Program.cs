using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using iTeco.Lib.Base;

using log4net;



namespace AK.Service.Test
{
	public class Item
	{
		public string Name;
		[XmlElement]
		public int[] Id;
	}

		
	static class Program
	{
		private static Random RND = new Random();
		private static ILog _logger = LogManager.GetLogger("1");
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			_logger.Debug("1");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form1 f1=new Form1();
			if (System.Environment.CommandLine.IndexOf("autostart")>-1) 
				f1.AutoStart = true;
			Application.Run(f1);
		}

	}
}
