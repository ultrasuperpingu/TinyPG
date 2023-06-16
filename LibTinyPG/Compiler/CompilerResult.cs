﻿// Copyright 2008 - 2010 Herre Kuijpers - <herre.kuijpers@gmail.com>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TinyPG.Debug;

namespace TinyPG.Compiler
{
	public class CompilerResult
	{
		public object Scanner;
		public IParser Parser;
		public IParseTree ParseTree;
		public string Output;
		public Assembly Assembly;
		public object Value;
		public List<IParseError> ParsingErrors;
		public List<IParseError> EvalErrors;
	}
}
