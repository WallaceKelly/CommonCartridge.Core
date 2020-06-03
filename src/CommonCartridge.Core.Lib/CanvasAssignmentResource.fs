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
        match title.StartsWith(prefix) with
        | true -> title.Substring(prefix.Length)
        | false -> title

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =

        if Array.length resource.Files < 2 then
            "Cannot create CanvasAssignmentResource without at least two files."
            |> CanvasResource.createFailMessage resource 
            |> failwith

        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            sprintf ": Cannot find file '%s'." fullPath
            |> CanvasResource.createFailMessage resource
            |> failwith

        let html = File.ReadAllText(fullPath)
        
        { CanvasAssignmentResource.Identifier = resource.Identifier
          Title = CanvasHtmlParsing.getTitle html |> stripPrefix
          Content = CanvasHtmlParsing.getBody html }
