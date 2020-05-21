namespace CommonCartridge

open System
open CommonCartridge.ImsccManifest

type ImsccResourceType =
     | Unknown = 0
     | Page = 1
     | File = 2
     | Assignment = 3
     | Quiz = 4
     | Discussion = 5
     | External = 6

module internal ImsccResourceType =

    let isOfResourceType (resource: Schema.Resource2) (resourceType: ImsccResourceType) = 
        match resourceType with
        | ImsccResourceType.Unknown ->
            resource.Type = "imsbasiclti_xmlv1p"
            || resource.Type = "imsbasiclti_xmlv1p0"
        // | ImsccResourceType.TextHeader -> resource.Type = ""
        | ImsccResourceType.Page ->
            resource.Type = "webcontent"
            && resource.Href.IsSome
            && resource.Href.Value.StartsWith("wiki_content")
        | ImsccResourceType.File ->
            resource.Type = "webcontent"
            && resource.Href.IsSome
            && resource.Href.Value.StartsWith("web_resources")
        | ImsccResourceType.Assignment ->
            resource.Type = "associatedcontent/imscc_xmlv1p1/learning-application-resource"
        | ImsccResourceType.Quiz ->
            resource.Type = "imsqti_xmlv1p2/imscc_xmlv1p1/assessment"
        | ImsccResourceType.Discussion ->
            resource.Type = "imsdt_xmlv1p1"
        | ImsccResourceType.External ->
            resource.Type = "imswl_xmlv1p1"
        | t -> failwithf "Unhandled resource type case: %A" t

    let getResourceType (resource: Schema.Resource2) =
        Enum.GetValues(typeof<ImsccResourceType>)
        |> Seq.cast<ImsccResourceType>
        |> Seq.where(isOfResourceType resource)
        |> Seq.tryExactlyOne
        |> function
            | Some(t) -> t
            | None -> failwithf "Could not determine type of resource %s" resource.Identifier

[<CLIMutable>]
type internal ImsccResource =
    { Identifier: string
      Type: ImsccResourceType
      Href: string
      Files: string[] }

module internal ImsccResource =

    let private createResource (resource: Schema.Resource2) =
        { ImsccResource.Identifier = resource.Identifier
          Type = ImsccResourceType.getResourceType resource
          Href = resource.Href |> Option.defaultValue ""
          Files = resource.Files |> Array.map(fun f -> f.Href) }

    let fromManifest (manifest: Schema.Manifest) =
        manifest.Resources.Resources
        |> Seq.map createResource

    let fromFile (imsccFile: ImsccFile) =
        imsccFile |> ImsccManifest.read |> fromManifest