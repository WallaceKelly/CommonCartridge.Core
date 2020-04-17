namespace CommonCartridge

open CommonCartridge.ImsccManifest

[<CLIMutable>]
type ImsccModule =
    { Title: string }

module ImsccModule =

    let private createModule (schemaManifest: Schema.Item2) =
        { ImsccModule.Title = schemaManifest.Title }

    let fromManifest (manifest: Schema.Manifest) =
        manifest.Organization
        |> Option.map(fun o -> o.Item)
        |> Option.map(fun i -> i.Items)
        |> Option.defaultValue [||]
        |> Seq.map(createModule)

    let fromFile = ImsccManifest.read >> fromManifest