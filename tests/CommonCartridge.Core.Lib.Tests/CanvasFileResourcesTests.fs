module CanvasFileResourcesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, 2)>]
let ``CanvasFileResource.ofCanvasResource can parse file resources.``(path: string) (count: int) =

    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    let canvasResources = manifest |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.File 

    // act
    let fileResources =
        canvasResources
        |> List.map(Option.map (CanvasFileResource.ofCanvasResource manifest))

    // assert
    Assert.Equal(count, fileResources.Length)
    for fr in fileResources do
        Assert.True(fr.IsSome)
        Assert.NotEmpty(fr.Value.FullPath)
        Assert.True(File.Exists fr.Value.FullPath)
