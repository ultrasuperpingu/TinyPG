TinyPG
======

The unofficial fork of the Tiny Parser Generator by Herre Kuijpers. It was first forked by [SickheadGames](https://github.com/SickheadGames/TinyPG), and this is a fork of the later.

It is an LL(1) recursive descent parser generator written in C# which can generate a scanner, parser, and parsetree file in either C#, VB, Java or C++ code. It's also a IDE allowing to design a grammar.

The original code and documentation can be found in the article ['A Tiny Parser Generator v1.2' on CodeProject](http://www.codeproject.com/Articles/28294/a-Tiny-Parser-Generator-v1-2).
  
The source code is licensed under the [Code Project Open License (CPOL)
](http://www.codeproject.com/info/cpol10.aspx).


### Features & Fixes since v1.3

These are the new features and fixes we have added to the original code:
 - New features:
   - Added Java and C++ support.
   - Added [TinyExe](https://www.codeproject.com/Articles/241830/a-Tiny-Expression-Evaluator) as an example (in C#, Java and C++)
   - Command line building of parsers.
   - Support for `[IgnoreCase]` flag on terminal symbols.
   - Support `RegexCompiled` option to don't use `RegexOptions.Compiled` in Scanner regexs
 - Fixes and small improvements:
   - Fix `ParseError` line numbers and reports it in the IDE.
   - Fix Regex tool updates flickering.
   - Display the offending character on unexpected token errors.
   - Display the list of expected tokens on errors.
   - Supports `var` keyword in syntax highlighting.
   - Upgraded to C# 8.x.

### Downloads

The latest source code can be found in [zip form here](https://github.com/ultrasuperpingu/TinyPG/archive/master.zip).
The lastest build can be found [in the releases page](https://github.com/ultrasuperpingu/TinyPG/releases/latest).

