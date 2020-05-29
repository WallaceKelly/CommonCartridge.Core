namespace CommonCartridge.Canvas

open System
open CommonCartridge

type CanvasActivity =
    | Unsupported of string
    | Page of CanvasPageResource
    | File of CanvasFileResource
    | Assignment of CanvasAssignmentResource 
    | Quiz
    | Discussion
    | External

module CanvasActivity =

    let private getByIdentifierInternal manifest moduleItemTitle (resource: CanvasResource) =

        let create ofResource toActivity =
            resource.Identifier
            |> CanvasResource.getByIdentifier manifest
            |> Option.map(ofResource manifest)
            |> Option.map(toActivity)

        match resource.Type with
        | CanvasResourceType.File -> create CanvasFileResource.ofCanvasResource CanvasActivity.File
        | CanvasResourceType.Page -> create CanvasPageResource.ofCanvasResource CanvasActivity.Page
        | CanvasResourceType.Assignment -> create CanvasAssignmentResource.ofCanvasResource CanvasActivity.Assignment
        | t ->
            moduleItemTitle
            |> sprintf "%s: %s" (t.ToString())
            |> Unsupported
            |> Some

    let getByIdentifier manifest identifier =
        identifier
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest "")

    let getByModuleItem manifest (item: CanvasModuleItem) =
        item.IdentifierRef
        |> CanvasResource.getByIdentifier manifest
        |> Option.bind(getByIdentifierInternal manifest item.ModuleItemTitle)