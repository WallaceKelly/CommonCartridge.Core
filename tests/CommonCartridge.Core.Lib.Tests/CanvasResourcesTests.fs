module CanvasResourceTests

open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData("Assignment 1", CanvasResourceType.Assignment)>]
[<InlineData("Assignment 2", CanvasResourceType.Assignment)>]
[<InlineData("Discussion 1", CanvasResourceType.Discussion)>]
[<InlineData("Discussion 2", CanvasResourceType.Discussion)>]
[<InlineData("External URL 1", CanvasResourceType.External)>]
[<InlineData("External URL 2", CanvasResourceType.External)>]
// These don't have resources apparently?
// [<InlineData("External Tool 1", CanvasResourceType.External)>]
// [<InlineData("External Tool 2", CanvasResourceType.External)>]
[<InlineData("file 1.txt", CanvasResourceType.File)>]
[<InlineData("file 2.txt", CanvasResourceType.File)>]
[<InlineData("Page 1", CanvasResourceType.Page)>]
[<InlineData("Page 2", CanvasResourceType.Page)>]
[<InlineData("Quiz 1", CanvasResourceType.Quiz)>]
[<InlineData("Quiz 2", CanvasResourceType.Quiz)>]
let ``CanvasResourceType.getResourceType can recognize activity types`` (title: string) (resourceType: CanvasResourceType) =
    // arrange
    use imsccFile = new ImsccFile(ImsccTestFilePaths.Modules)
    let manifest = ImsccManifest.read imsccFile
    let modul =
        manifest
        |> CanvasModule.fromManifest 
        |> Seq.collect(fun m -> m.Items)
        |> Seq.where(fun m -> m.ModuleItemTitle = title)
        |> Seq.exactlyOne
    // act
    let resource =
        manifest
        |> CanvasResource.fromManifest
        |> Seq.where(fun r -> r.Identifier = modul.IdentifierRef)
        |> Seq.exactlyOne 
    // assert
    Assert.Equal(resourceType, resource.Type)

