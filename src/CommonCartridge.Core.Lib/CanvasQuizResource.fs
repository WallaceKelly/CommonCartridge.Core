namespace CommonCartridge.Canvas

open System.IO
open CommonCartridge

[<CLIMutable>]
type CanvasQuizResource =
    { Identifier: string
      Title: string
      Content: string }

module internal CanvasQuizResource =

    let ofCanvasResource (manifest: ImsccManifest) (resource: CanvasResource) =
        if Array.length resource.Dependencies < 1 then
            CanvasResource.failfor resource "Cannot create CanvasQuizResource without at least one dependency."
        
        let dependency =
            resource.Dependencies.[0]
            |> CanvasResource.getByIdentifier manifest

        if dependency.IsNone then
            CanvasResource.failfor resource "Cannot find dependency for Quiz."

        if Array.length dependency.Value.Files = 0 then
            CanvasResource.failfor resource "Cannot create CanvasQuizResource without at least one file in the dependency resource."

        let fullPath = Path.Combine(manifest.ExtractedFolder, dependency.Value.Files.[0])
        if not(File.Exists fullPath) then
            CanvasResource.failfor resource (sprintf ": Cannot find file '%s'." fullPath)

        ImsccQuiz.read manifest dependency.Value.Files.[0]
        |> function
            | None ->
                CanvasResource.failfor dependency.Value (sprintf ": Cannot load file '%s'." fullPath)
                failwith "Error"
            | Some(quiz) ->
                let result = 
                    { CanvasQuizResource.Identifier = quiz.Identifier
                      Title = quiz.Title |> Option.defaultValue "Unnamed Quiz"
                      Content = quiz.Description |> Option.defaultValue "" }
                result
