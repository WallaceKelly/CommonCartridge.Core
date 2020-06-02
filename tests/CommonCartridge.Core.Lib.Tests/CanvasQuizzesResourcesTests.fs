module CanvasQuizzesResourcesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, "Quiz 1", "<p>This is the content of Quiz 1.</p>")>]
[<InlineData(ImsccTestFilePaths.Modules, "Quiz 2", "<p>This is the content of Quiz 2.</p>")>]
let ``CanvasQuizResource.ofCanvasResource can parse Quiz resources.``(path: string) (title: string) (content: string) =

    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    let canvasResources = manifest |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.Quiz

    // act
    let quizResources =
        canvasResources
        |> List.map(Option.map (CanvasQuizResource.ofCanvasResource manifest))

    // assert
    let isMatch (pr: CanvasQuizResource option) = pr.IsSome && pr.Value.Title = title
    Assert.True(quizResources |> Seq.forall(Option.isSome))
    Assert.True(quizResources |> Seq.exists(isMatch))
    let quiz = quizResources |> Seq.find(isMatch)
    Assert.Equal(content, quiz.Value.Content)