namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasFileResource =
    { Identifier: string
      FullPath: string }

module internal CanvasFileResource =

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =

        if Array.isEmpty resource.Files then
            "Cannot create CanvasFileResource without a file reference."
            |> CanvasResource.createFailMessage resource 
            |> failwith

        if Array.length resource.Files > 1 then
            "Cannot create CanvasFileResource with multiple files."
            |> CanvasResource.createFailMessage resource 
            |> failwith

        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            sprintf "Cannot find file '%s'." fullPath
            |> CanvasResource.createFailMessage resource
            |> failwith

        { CanvasFileResource.Identifier = resource.Identifier
          FullPath = fullPath }
