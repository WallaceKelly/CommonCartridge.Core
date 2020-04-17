module WordModulesTests

open Xunit
open CommonCartridge.Word
open System.IO
open CommonCartridge

[<Fact>]
let ``WordModules.append appends a module to a Word file`` () =
    // arrange
    let path = Path.GetTempFileName()
    try
        let wordFile = ImsccWordFile.create path
        let modul = { ImsccModule.Title = "Module A" }
        // act
        modul |> WordModule.append wordFile
        wordFile |> ImsccWordFile.saveFile
        // assert
        let wordFile2 = ImsccWordFile.openFile path
        Assert.True(
            wordFile2.Document.Paragraphs
            |> Seq.exists(fun p ->
                p.Runs
                |> Seq.exists(fun r -> r.Text.Contains("Module A")))
        )
    finally
        File.Delete(path)