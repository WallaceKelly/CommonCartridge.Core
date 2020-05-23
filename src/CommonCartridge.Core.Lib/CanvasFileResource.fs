namespace CommonCartridge.Canvas

open System
open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasFileResource =
    { Identifier: string
      FullPath: string }

module internal CanvasFileResource =

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =
        if Array.isEmpty resource.Files then
            raise(ArgumentException("Cannot create CanvasFileResource without a file reference."))
        if Array.length resource.Files > 1 then
            raise(ArgumentException("Cannot create CanvasFileResource with multiple files."))
        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            raise(FileNotFoundException(sprintf "Cannot find file '%s'." fullPath))
        { CanvasFileResource.Identifier = resource.Identifier
          FullPath = fullPath }