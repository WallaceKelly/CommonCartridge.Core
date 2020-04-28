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
        let modul = { ImsccModule.Title = "Module A"; Identifier = ""; Items = [||] }
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

[<Fact>]
let ``WordModules.append appends a activities to a Word file`` () =
    // arrange
    let path = Path.GetTempFileName()
    try
        let wordFile = ImsccWordFile.create path
        let activity = { ImsccModuleItem.Title = "Activity 1"; Identifier = ""; IdentifierRef = "" }
        let modul = { ImsccModule.Title = "Module A"; Identifier = ""; Items = [| activity |] }
        // act
        modul |> WordModule.append wordFile
        wordFile |> ImsccWordFile.saveFile
        // assert
        let wordFile2 = ImsccWordFile.openFile path
        Assert.True(
            wordFile2.Document.Paragraphs
            |> Seq.exists(fun p ->
                p.Runs
                |> Seq.exists(fun r -> r.Text.Contains("Activity 1")))
        )
    finally
        File.Delete(path)