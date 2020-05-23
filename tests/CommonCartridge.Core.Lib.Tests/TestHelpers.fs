module CommonCartridge.TestHelpers

open System.IO
open System.Xml.Linq
open CommonCartridge.Canvas

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

let internal getCanvasResourcesOfType (resourceType: CanvasResourceType) (manifest: ImsccManifest) =
    manifest
    |> CanvasModules.inManifest 
    |> Seq.collect(fun m -> m.Items
                            |> Seq.where(fun i -> i.Type = resourceType))
    |> Seq.map(fun i -> i.IdentifierRef)
    |> Seq.map(CanvasResource.getByIdentifier manifest)
    |> Seq.toList


