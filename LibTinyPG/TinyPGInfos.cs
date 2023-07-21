using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TinyPG
{
	public static  class TinyPGInfos
	{
		public static string Version
		{
			get
			{
				string verStr="";
				var ver = Assembly.GetExecutingAssembly().GetName().Version;
				verStr += ver.Major;
				verStr += "." + ver.Minor;
				verStr = verStr.TrimStart('.');
				return verStr;
			}
		}
	}
}
