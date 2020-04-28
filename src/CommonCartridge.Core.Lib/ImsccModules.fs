namespace CommonCartridge

open CommonCartridge.ImsccManifest

[<CLIMutable>]
type ImsccModuleItem =
    { Title: string
      Identifier: string
      IdentifierRef: string }

[<CLIMutable>]
type ImsccModule =
    { Title: string
      Identifier: string
      Items: ImsccModuleItem[] }

module ImsccModule =

    let private createModuleItem (item2: Schema.Item2) =
        { ImsccModuleItem.Title = item2.Title
          Identifier = item2.Identifier
          IdentifierRef = item2.Identifierref |> Option.defaultValue "" }

    let private createModule (item2: Schema.Item2) =
        { ImsccModule.Title = item2.Title
          Identifier = item2.Identifier
          Items = item2.Items |> Array.map createModuleItem }

    let fromManifest (manifest: Schema.Manifest) =
        manifest.Organization
        |> Option.map(fun o -> o.Item)
        |> Option.map(fun i -> i.Items)
        |> Option.defaultValue [||]
        |> Seq.map createModule

    let fromFile = ImsccManifest.read >> fromManifest