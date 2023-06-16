using System;
using System.Collections.Generic;
using System.Text;
using TinyPG.Compiler;

namespace TinyPG.CodeGenerators
{
	public class BaseGenerator
	{
		protected List<string> templateFiles = new List<string>();

		public BaseGenerator(params string[] templateNames)
		{
			this.templateFiles = new List<string>(templateNames);

		}
		public virtual List<string> TemplateFiles
		{
			get { return this.templateFiles; }
			set { this.templateFiles = value; }
		}

		protected string ReplaceDirectiveAttributes(string fileContent, Directive directive)
		{
			foreach(var att in directive)
			{
				fileContent = fileContent.Replace("<%"+att.Key+"%>", att.Value);
			}
			return fileContent;
		}
	}
}
