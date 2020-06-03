
namespace CommonCartridge

open System.IO
open FSharp.Data

module ImsccQuiz =

    let [<Literal>] private SchemaUri = "https://canvas.instructure.com/xsd/cccv1p0.xsd"

    type internal Schema = XmlProvider< Schema = SchemaUri >

    let read (manifest: ImsccManifest) (identifier: string) =
        let filePath = Path.Combine(manifest.ExtractedFolder, identifier)
        Schema.Load(filePath).Quiz