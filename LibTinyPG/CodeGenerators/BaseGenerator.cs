using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TinyPG.Parsing;

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

		protected string GenerateComment(object[] objects, string Indent, bool isJavadoc = false)
		{
			StringBuilder sb = new StringBuilder();
			if (isJavadoc)
			{
				sb.Append(Indent).AppendLine("/** ");
			}
			foreach (var o in objects)
			{
				sb.Append(Indent).Append(isJavadoc ? "* " : "/// ").AppendLine(Helper.LiteralToUnescaped(o.ToString()));
			}
			if (isJavadoc)
			{
				sb.Append(Indent).AppendLine("*/ ");
			}
			return sb.ToString().TrimEnd();
		}

		protected Dictionary<string, string> GetTemplateFilesPath(Grammar Grammar, string directiveName)
		{
			Dictionary<string, string> _templateFiles = new Dictionary<string, string>();
			string templatePath = Grammar.GetTemplatePath();
			if (string.IsNullOrEmpty(templatePath))
				throw new Exception("Template path not found:" + Grammar.Directives["TinyPG"]["TemplatePath"]);
			List<string> files;
			if (Grammar.Directives["ParseTree"].ContainsKey("TemplateFiles"))
			{
				var templateFilesString = Grammar.Directives[directiveName]["TemplateFiles"];
				files = new List<string>(templateFilesString.Split(','));
				for (int i = 0; i < files.Count; i++)
				{
					if (string.IsNullOrWhiteSpace(files[i]))
					{
						files.RemoveAt(i);
						i--;
						continue;
					}
					files[i] = files[i].Trim();
				}
			}
			else
			{
				files=templateFiles;
			}
			for (int i = 0; i < files.Count; i++)
			{
				var f = Path.Combine(templatePath, files[i]);
				if (!File.Exists(f))
				{
					throw new Exception("Template file "+files[i]+" does not exist.");
				}
				_templateFiles.Add(files[i], f);
			}

			return _templateFiles;
		}
	}
}
