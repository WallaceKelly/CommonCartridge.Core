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

        if Array.isEmpty resource.Dependencies then
            "Cannot create CanvasQuizResource without at least one dependency."
            |> CanvasResource.createFailMessage resource 
            |> failwith
        
        let dependency =
            resource.Dependencies.[0]
            |> CanvasResource.getByIdentifier manifest
            |> Option.defaultWith (fun () ->
                    "Cannot find dependency for quiz."
                    |> CanvasResource.createFailMessage resource
                    |> failwith
                )

        if Array.isEmpty dependency.Files then
            "Cannot create CanvasQuizResource without at least one file in the dependency resource."
            |> CanvasResource.createFailMessage resource
            |> failwith

        let fullPath = Path.Combine(manifest.ExtractedFolder, dependency.Files.[0])
        if not(File.Exists fullPath) then
            sprintf "Cannot find file '%s'." fullPath
            |> CanvasResource.createFailMessage resource 
            |> failwith

        ImsccQuiz.read manifest dependency.Files.[0]
        |> Option.map(fun q ->
            { CanvasQuizResource.Identifier = q.Identifier 
              Title = q.Title |> Option.defaultValue "Untitled Quiz"
              Content = q.Description |> Option.defaultValue "" })
        |> Option.defaultWith(fun () ->
                sprintf ": Cannot load file '%s'." fullPath
                |> CanvasResource.createFailMessage dependency
                |> failwith
            )
