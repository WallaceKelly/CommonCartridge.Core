module CommonCartridge.TestHelpers

open System.IO
open System.Xml.Linq

let savesIdenticalXml (srcFilePath: string) (data: XElement) =
    let dstFilePath = Path.GetTempFileName()
    try
        data.Save(dstFilePath)
        let xdocSrc = XDocument.Load(srcFilePath)
        let xdocDst = XDocument.Load(dstFilePath)
        XNode.DeepEquals(xdocSrc, xdocDst)
    finally
        if File.Exists dstFilePath then
            File.Delete dstFilePath


