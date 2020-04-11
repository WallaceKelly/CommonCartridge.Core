module CommonCartridge.ManifestTests

open Xunit
open System.IO
open XmlDiffLib

[<Fact>]
let ``Loads empty manifest without throwing``() =
    Manifest.Load("imsmanifest.xml")
    |> ignore

[<Fact>]
let ``Saves unmodified empty manifest as identical file``() =
    
    let srcFilePath = "imsmanifest.xml"
    let dstFilePath = Path.GetTempFileName()
    try
        let manifest = Manifest.Load(srcFilePath)
        manifest.XElement.Save(dstFilePath)
        let srcFile = File.ReadAllText(srcFilePath)
        let dstFile = File.ReadAllText(srcFilePath)
        let diff = new XmlDiff(srcFile, dstFile)
        diff.CompareDocuments(XmlDiffOptions(TwoWayMatch = true)) |> ignore
        Assert.Empty(diff.DiffNodeList)
    finally
        if File.Exists dstFilePath then
            File.Delete dstFilePath