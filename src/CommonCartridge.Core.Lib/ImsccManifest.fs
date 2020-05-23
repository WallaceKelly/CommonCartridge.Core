namespace CommonCartridge

open System.IO
open FSharp.Data

module ImsccManifest =

    let [<Literal>] private SchemaUri = "http://www.imsglobal.org/profile/cc/ccv1p1/ccv1p1_imscp_v1p2_v1p0.xsd"

    type internal Schema = XmlProvider< Schema = SchemaUri >

    // aliases to prettify the API
    and Resource = Schema.Resource2
    and Item = Schema.Item2
    and Organization = Schema.Organization
    and ManifestFile =
        { ExtractedFolder: string
          Organization: Schema.Organization option
          Resources: Schema.Resource2[] }

    let read (imsccFile: ImsccFile) =
        let filePath = Path.Combine(imsccFile.ExtractedFolder, "imsmanifest.xml")
        let manifest = Schema.Load(filePath).Manifest.Value
        { ExtractedFolder = imsccFile.ExtractedFolder
          Organization = manifest.Organization
          Resources = manifest.Resources.Resources }

type ImsccManifest = ImsccManifest.ManifestFile