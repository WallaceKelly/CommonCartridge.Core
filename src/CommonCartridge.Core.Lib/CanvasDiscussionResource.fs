namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasDiscussionResource =
    { Identifier: string
      Title: string
      Content: string }

module internal CanvasDiscussionResource =

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =

        if Array.isEmpty resource.Files then
            "Cannot create CanvasDiscussionResource without at least one file in the dependency resource."
            |> CanvasResource.createFailMessage resource
            |> failwith

        let fullPath = Path.Combine(manifest.ExtractedFolder, resource.Files.[0])
        if not(File.Exists fullPath) then
            sprintf "Cannot find file '%s'." fullPath
            |> CanvasResource.createFailMessage resource 
            |> failwith

        let discussion = ImsccDiscussion.read manifest resource.Files.[0]
        { CanvasDiscussionResource.Identifier = resource.Identifier
          Title = discussion.Title
          Content = discussion.Text.Value }
