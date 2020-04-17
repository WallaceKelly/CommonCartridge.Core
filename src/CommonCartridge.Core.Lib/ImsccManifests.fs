namespace CommonCartridge

open System.IO
open FSharp.Data


module ImsccManifest =

    let [<Literal>] private SchemaUri = "http://www.imsglobal.org/profile/cc/ccv1p1/ccv1p1_imscp_v1p2_v1p0.xsd"

    type internal Schema = XmlProvider< Schema = SchemaUri >

    let read (imsccFile: ImsccFile) =
        let filePath = Path.Combine(imsccFile.ExtractedFolder, "imsmanifest.xml")
        Schema.Load(filePath).Manifest.Value
