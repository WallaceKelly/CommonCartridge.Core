namespace CommonCartridge

open System
open CommonCartridge.ImsccManifest
type internal ResourceDictionary = System.Collections.Generic.IDictionary<string, ImsccResource>

[<CLIMutable>]
type ImsccModuleItem =
    { ModuleTitle: string
      Identifier: string
      IdentifierRef: string
      Type: ImsccResourceType }

[<CLIMutable>]
type ImsccModule =
    { Title: string
      Identifier: string
      Items: ImsccModuleItem[] }

module ImsccModule =

    let private createModuleItem (item2: Schema.Item2, resource: ImsccResource option) =
        let typ = 
            resource
            |> Option.map(fun r -> r.Type)
            |> Option.defaultValue ImsccResourceType.Unknown
        { ImsccModuleItem.ModuleTitle = item2.Title
          Identifier = item2.Identifier
          IdentifierRef = item2.Identifierref |> Option.defaultValue ""
          Type = typ }

    let private createModule (resources: ResourceDictionary) (item2: Schema.Item2) =

        let getResource (identifierRef: string option) =
            identifierRef
            |> Option.bind(fun r -> if String.IsNullOrWhiteSpace r then None else Some r)
            |> Option.bind(fun r -> if resources.ContainsKey(r) then Some(resources.[r]) else None)

        { ImsccModule.Title = item2.Title
          Identifier = item2.Identifier
          Items =
            item2.Items
            |> Array.map(fun i -> i, getResource i.Identifierref)
            |> Array.map createModuleItem }

    let fromManifest (manifest: Schema.Manifest) =

        let resources =
            manifest
            |> ImsccResource.fromManifest
            |> Seq.map(fun r -> r.Identifier, r)
            |> dict

        manifest.Organization
        |> Option.map(fun o -> o.Item)
        |> Option.map(fun i -> i.Items)
        |> Option.defaultValue [||]
        |> Seq.map (createModule resources)

    let fromFile (imsccFile: ImsccFile) =
        imsccFile |> ImsccManifest.read |> fromManifest