namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasFileResource =
    { Identifier: string
      FullPath: string }

module internal CanvasFileResource =

    let private failfor (resource: CanvasResource) (msg: string) =
        failwith (sprintf "%s (%s)" msg resource.Identifier)
        ()

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =
        if Array.isEmpty resource.Files then
            failfor resource "Cannot create CanvasFileResource without a file reference."
        if Array.length resource.Files > 1 then
            failfor resource "Cannot create CanvasFileResource with multiple files."
        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            failfor resource (sprintf "Cannot find file '%s'." fullPath)

        { CanvasFileResource.Identifier = resource.Identifier
          FullPath = fullPath }