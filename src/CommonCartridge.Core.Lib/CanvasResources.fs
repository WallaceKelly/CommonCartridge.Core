namespace CommonCartridge.Canvas

open System
open CommonCartridge

type CanvasResourceType =
     | Unknown = 0
     | Page = 1
     | File = 2
     | Assignment = 3
     | Quiz = 4
     | Discussion = 5
     | External = 6

module internal CanvasResourceType =

    let isOfResourceType (resource: ImsccManifest.Resource) (resourceType: CanvasResourceType) = 
        match resourceType with
        | CanvasResourceType.Unknown ->
            resource.Type = "imsbasiclti_xmlv1p"
            || resource.Type = "imsbasiclti_xmlv1p0"
        // | ImsccResourceType.TextHeader -> resource.Type = ""
        | CanvasResourceType.Page ->
            resource.Type = "webcontent"
            && resource.Href.IsSome
            && resource.Href.Value.StartsWith("wiki_content")
        | CanvasResourceType.File ->
            resource.Type = "webcontent"
            && resource.Href.IsSome
            && resource.Href.Value.StartsWith("web_resources")
        | CanvasResourceType.Assignment ->
            resource.Type = "associatedcontent/imscc_xmlv1p1/learning-application-resource"
        | CanvasResourceType.Quiz ->
            resource.Type = "imsqti_xmlv1p2/imscc_xmlv1p1/assessment"
        | CanvasResourceType.Discussion ->
            resource.Type = "imsdt_xmlv1p1"
        | CanvasResourceType.External ->
            resource.Type = "imswl_xmlv1p1"
        | t -> failwithf "Unhandled resource type case: %A" t

    let getResourceType (resource: ImsccManifest.Resource) =
        Enum.GetValues(typeof<CanvasResourceType>)
        |> Seq.cast<CanvasResourceType>
        |> Seq.where(isOfResourceType resource)
        |> Seq.tryExactlyOne
        |> function
            | Some(t) -> t
            | None -> failwithf "Could not determine type of resource %s" resource.Identifier

[<CLIMutable>]
type internal CanvasResource =
    { Identifier: string
      Type: CanvasResourceType
      Href: string
      Files: string[] }

module internal CanvasResource =

    let createResource (resource: ImsccManifest.Resource) =
        { CanvasResource.Identifier = resource.Identifier
          Type = CanvasResourceType.getResourceType resource
          Href = resource.Href |> Option.defaultValue ""
          Files = resource.Files |> Array.map(fun f -> f.Href) }

    let getByIdentifier (manifest: ImsccManifest) (identifier: string) =
        manifest.Resources
        |> Seq.where(fun r -> r.Identifier = identifier)
        |> Seq.tryExactlyOne
        |> Option.map createResource

module internal CanvasResources =

    let inManifest (manifest: ImsccManifest) =
        manifest.Resources
        |> Seq.map(CanvasResource.createResource)

