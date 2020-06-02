namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasAssignmentResource =
    { Identifier: string
      Title: string
      Content: string }

module internal CanvasAssignmentResource =

    let private stripPrefix (title: string) =
        let prefix = "Assignment: "
        if title.StartsWith(prefix) then
            title.Substring(prefix.Length)
        else
            title

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =
        if Array.length resource.Files < 2 then
            CanvasResource.failfor resource "Cannot create CanvasAssignmentResource without at least two files."
        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            CanvasResource.failfor resource (sprintf ": Cannot find file '%s'." fullPath)

        let html = File.ReadAllText(fullPath)
        
        { CanvasAssignmentResource.Identifier = resource.Identifier
          Title = CanvasHtmlParsing.getTitle html |> stripPrefix
          Content = CanvasHtmlParsing.getBody html }
