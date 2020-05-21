module CanvasModulesTests

open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``CanvasModule.fromFile can read modules without throwing`` (path: string) =
    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    // act
    // assert
    manifest |> CanvasModule.fromManifest |> ignore

[<Theory>]
[<InlineData("Assignment 1", CanvasResourceType.Assignment)>]
[<InlineData("Assignment 2", CanvasResourceType.Assignment)>]
[<InlineData("Discussion 1", CanvasResourceType.Discussion)>]
[<InlineData("Discussion 2", CanvasResourceType.Discussion)>]
[<InlineData("External URL 1", CanvasResourceType.External)>]
[<InlineData("External URL 2", CanvasResourceType.External)>]
[<InlineData("file 1.txt", CanvasResourceType.File)>]
[<InlineData("file 2.txt", CanvasResourceType.File)>]
[<InlineData("Page 1", CanvasResourceType.Page)>]
[<InlineData("Page 2", CanvasResourceType.Page)>]
[<InlineData("Quiz 1", CanvasResourceType.Quiz)>]
[<InlineData("Quiz 2", CanvasResourceType.Quiz)>]
let ``CanvasModule.fromFile can read module titles`` (title: string) (resourceType: CanvasResourceType) =
    // arrange
    use imsccFile = new ImsccFile(ImsccTestFilePaths.Modules)
    let manifest = ImsccManifest.read imsccFile
    // act
    manifest
    |> CanvasModule.fromManifest
    |> Seq.collect(fun m -> m.Items)
    |> Seq.where(fun i -> i.Type = resourceType)
    |> Seq.where(fun i -> i.ModuleItemTitle = title)
    |> Seq.exactlyOne // assert
    |> ignore
