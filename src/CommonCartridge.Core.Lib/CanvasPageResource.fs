namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasPageResource =
    { Identifier: string
      Title: string
      Content: string }

module internal CanvasPageResource =

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =

        if Array.isEmpty resource.Files then
            "Cannot create CanvasPageResource without a file reference."
            |> CanvasResource.createFailMessage resource
            |> failwith

        if Array.length resource.Files > 1 then
            "Cannot create CanvasPageResource with multiple files."
            |> CanvasResource.createFailMessage resource 
            |> failwith

        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            sprintf "Cannot find file '%s'." fullPath
            |> CanvasResource.createFailMessage resource
            |> failwith

        let html = File.ReadAllText(fullPath)

        { CanvasPageResource.Identifier = resource.Identifier
          Title = CanvasHtmlParsing.getTitle html
          Content = CanvasHtmlParsing.getBody html }
