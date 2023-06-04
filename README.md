TinyPG
======

The unofficial fork of the Tiny Parser Generator by Herre Kuijpers, then forked by [SickheadGames](https://github.com/SickheadGames/TinyPG).

It is an LL(1) recursive descent parser generator written in C# which can generate a scanner, parser, and parsetree file in either C#, VB, Java or C++ code.

The original code and documentation can be found in the article ['A Tiny Parser Generator v1.2' on CodeProject](http://www.codeproject.com/Articles/28294/a-Tiny-Parser-Generator-v1-2
).
  
The source code is licensed under the [Code Project Open License (CPOL)
](http://www.codeproject.com/info/cpol10.aspx).


### Features & Fixes

These are the new features and fixes we have added to the original code:
 - New features:
   - Added Java and C++ support.
   - Command line building of parsers.
   - Support for `[IgnoreCase]` flag on terminal symbols.
   - Support RegexCompiled option to don't use RegexOptions.Compiled in Scanner regexs
 - Fixes and small improvements:
   - Fix `ParseError` line numbers and reports it in the IDE.
   - Fix Regex tool updates flickering.
   - Display the offending character on Unexpected token errors.
   - Display the list of expected tokens on errors.
   - Supports `var` keyword in syntax highlighting.
   - Upgraded to C# 8.x.

### Downloads

The latest source code can be found in [zip form here](https://github.com/ultrasuperpingu/TinyPG/archive/master.zip).

