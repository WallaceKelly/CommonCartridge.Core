module CanvasPageResourcesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, "Page 1", "<p>This is the content of Page 1.</p>")>]
[<InlineData(ImsccTestFilePaths.Modules, "Page 2", "<p>This is the content of Page 2.</p>")>]
let ``CanvasPageResource.ofCanvasResource can parse page resources.``(path: string) (title: string) (content: string) =

    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    let canvasResources = manifest |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.Page

    // act
    let pageResources =
        canvasResources
        |> List.map(Option.map (CanvasPageResource.ofCanvasResource manifest))

    // assert
    let isMatch (pr: CanvasPageResource option) = pr.IsSome && pr.Value.Title = title
    Assert.True(pageResources |> Seq.forall(Option.isSome))
    Assert.True(pageResources |> Seq.exists(isMatch))
    let page = pageResources |> Seq.find(isMatch)
    Assert.Equal(content, page.Value.Content)