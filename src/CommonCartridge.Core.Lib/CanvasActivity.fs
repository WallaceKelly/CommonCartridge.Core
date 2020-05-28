namespace CommonCartridge.Canvas

open System

type CanvasActivity =
    | Unsupported of string
    | Page of CanvasPageResource
    | File of CanvasFileResource
    | Assignment of CanvasAssignmentResource 
    | Quiz
    | Discussion
    | External

module CanvasActivity =

    let private getCanvasFileActivity manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.map(CanvasFileResource.ofCanvasResource manifest)
        |> Option.map(CanvasActivity.File)

    let private getCanvasPageActivity manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.map(CanvasPageResource.ofCanvasResource manifest)
        |> Option.map(CanvasActivity.Page)

    let private getCanvasAssignmentActivity manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.map(CanvasAssignmentResource.ofCanvasResource manifest)
        |> Option.map(CanvasActivity.Assignment)

    let private getByIdentifierInternal manifest (moduleItemTitle: string) (resource: CanvasResource) =
        match resource.Type with
        | CanvasResourceType.File -> getCanvasFileActivity manifest resource.Identifier
        | CanvasResourceType.Page -> getCanvasPageActivity manifest resource.Identifier
        | CanvasResourceType.Assignment -> getCanvasAssignmentActivity manifest resource.Identifier
        | t -> Some(Unsupported(sprintf "%s: %s" (t.ToString()) moduleItemTitle))

    let getByIdentifier manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest "")

    let getByModuleItem manifest (item: CanvasModuleItem) =
        item.IdentifierRef
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest item.ModuleItemTitle)
    