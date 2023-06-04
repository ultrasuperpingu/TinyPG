Imports System
Imports SimpleExpr.TinyPG

Module Program
    Sub Main(args As String())
        Dim context = New Dictionary(Of String, Integer)
        context.Add("_5", 5)
        context.Add("_15", 15)
        Dim p = New Parser(New Scanner())
        Dim input = "_5*3+_15/2"
        Dim tree = p.Parse(input)
        tree.Context = context
        Console.WriteLine(input + " = " + tree.Eval().ToString)
    End Sub
End Module
