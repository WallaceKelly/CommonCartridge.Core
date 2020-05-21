namespace CommonCartridge.Canvas

open System
open CommonCartridge

type internal ResourceDictionary = System.Collections.Generic.IDictionary<string, CanvasResource>

[<CLIMutable>]
type CanvasModule =
    { Title: string
      Identifier: string
      Items: CanvasModuleItem[] }

module CanvasModule =

    let private createModuleItem (item: ImsccManifest.Item, resource: CanvasResource option) =
        let typ = 
            resource
            |> Option.map(fun r -> r.Type)
            |> Option.defaultValue CanvasResourceType.Unknown
        { CanvasModuleItem.ModuleItemTitle = item.Title
          Identifier = item.Identifier
          IdentifierRef = item.Identifierref |> Option.defaultValue ""
          Type = typ }

    let private createModule (resources: ResourceDictionary) (item: ImsccManifest.Item) =

        let getResource (identifierRef: string option) =
            identifierRef
            |> Option.bind(fun r -> if String.IsNullOrWhiteSpace r then None else Some r)
            |> Option.bind(fun r -> if resources.ContainsKey(r) then Some(resources.[r]) else None)

        { CanvasModule.Title = item.Title
          Identifier = item.Identifier
          Items =
            item.Items
            |> Array.map(fun i -> i, getResource i.Identifierref)
            |> Array.map createModuleItem }

    let fromManifest (manifest: ImsccManifest) =

        let resources =
            manifest
            |> CanvasResource.fromManifest
            |> Seq.map(fun r -> r.Identifier, r)
            |> dict

        manifest.Organization
        |> Option.map(fun o -> o.Item)
        |> Option.map(fun i -> i.Items)
        |> Option.defaultValue [||]
        |> Seq.map (createModule resources)