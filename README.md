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

 - Support for `[IgnoreCase]` flag on terminal symbols.
 - Syntax highlighting now supports `var` keyword.
 - `ParseError` now has correct line numbers.
 - Regex tool now updates live without flicker.
 - The IDE will now display the error line number in the output.
 - Production rules without a code block will by default evaluate their sub-rules.
 - New `[FileAndLine]` flag for redefining the file and line number reported in errors.
 - IDE now uses C# 3.x compiler when testing the generated parser code.
 - Command line building of parsers.
 - IDE expression evaluator now include line and column numbers in errors.
 - Unexpected token errors now display the offending character.
 - We now always show the list of expected tokens on errors.
 - Added Java and C++ support
 - Added some backtracking when optionals terms (?, *, +) fails

### Downloads

The latest source code can be found in [zip form here](https://github.com/ultrasuperpingu/TinyPG/archive/master.zip).

