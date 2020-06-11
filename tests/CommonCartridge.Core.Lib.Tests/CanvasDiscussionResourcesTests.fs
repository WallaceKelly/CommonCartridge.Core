module CanvasDiscussionResourcesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, "Discussion 1", "<p>This is the content of Discussion 1.</p>")>]
[<InlineData(ImsccTestFilePaths.Modules, "Discussion 2", "<p>This is the content of Discussion 2.</p>")>]
let ``CanvasDiscussionResource.ofCanvasResource can parse Discussion resources.``(path: string) (title: string) (content: string) =

    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    let canvasResources = manifest |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.Discussion

    // act
    let discussionResources =
        canvasResources
        |> List.map(Option.map (CanvasDiscussionResource.ofCanvasResource manifest))

    // assert
    let isMatch (pr: CanvasDiscussionResource option) = pr.IsSome && pr.Value.Title = title
    Assert.True(discussionResources |> Seq.forall(Option.isSome))
    Assert.True(discussionResources |> Seq.exists(isMatch))
    let discussion = discussionResources |> Seq.find(isMatch)
    Assert.Equal(content, discussion.Value.Content)