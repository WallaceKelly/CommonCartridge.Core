namespace CommonCartridge

open System.IO
open FSharp.Data

module ImsccWebLink =

    let [<Literal>] private SchemaUri = "http://www.imsglobal.org/profile/cc/ccv1p1/ccv1p1_imswl_v1p1.xsd"

    type internal Schema = XmlProvider< Schema = SchemaUri >

    let read (manifest: ImsccManifest) (identifier: string) =
        let filePath = Path.Combine(manifest.ExtractedFolder, identifier)
        Schema.Load(filePath)


