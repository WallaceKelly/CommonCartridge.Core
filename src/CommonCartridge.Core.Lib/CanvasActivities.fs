namespace CommonCartridge.Canvas

open System

type CanvasActivity =
    | Unsupported of string
    | Page
    | File of CanvasFileResource
    | Assignment 
    | Quiz
    | Discussion
    | External

module CanvasActivity =

    let private getCanvasFileActivity manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.map(CanvasFileResource.fromResource manifest)
        |> Option.map(CanvasActivity.File)

    let private getByIdentifierInternal manifest (moduleItemTitle: string) (resource: CanvasResource) =
        match resource.Type with
        | CanvasResourceType.File -> getCanvasFileActivity manifest resource.Identifier
        | t -> Some(Unsupported(sprintf "%s: %s" (t.ToString()) moduleItemTitle))

    let getByIdentifier manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest "")

    let getByModuleItem manifest (item: CanvasModuleItem) =
        item.IdentifierRef
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest item.ModuleItemTitle)
    