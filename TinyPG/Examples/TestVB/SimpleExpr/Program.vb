Imports System
Imports SimpleExpr.TinyPG

Module Program
    Sub Main(args As String())
        Dim context = New Dictionary(Of String, Integer)
        context.Add("_5", 5)
        context.Add("_15", 15)
        Dim p = New Parser(New Scanner())
        Dim tree = p.Parse("_5*3+_15/2")
        tree.Context = context
        Console.WriteLine(tree.Eval())
    End Sub
End Module
