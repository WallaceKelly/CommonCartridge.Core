module CanvasActivitiesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, 2)>]
let ``CanvasActivity.getByIdentifier can load file activities``(path: string) (count: int) =

    // arrange
    use doc = new ImsccFile(path)
    let manifest = ImsccManifest.read doc
    let fileIdentifiers =
        manifest
        |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.File 
        |> Seq.choose(Option.map(fun r -> r.Identifier))
        |> Seq.toList

    // act
    let fileActivities = fileIdentifiers |> List.map(CanvasActivity.getByIdentifier manifest)

    // assert
    Assert.Equal(count, fileActivities.Length)
    for a in fileActivities do
        match a.Value with
        | CanvasActivity.File(fa) ->
            Assert.NotEmpty(fa.FullPath)
            Assert.True(File.Exists(fa.FullPath))
        | x -> failwithf "Incorrect content type: %A" x