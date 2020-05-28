module CanvasAssignmentResourcesTests

open System.IO
open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.Modules, "Assignment: Assignment 1", "<p>This is the content of Assignment 1.</p>")>]
[<InlineData(ImsccTestFilePaths.Modules, "Assignment: Assignment 2", "<p>This is the content of Assignment 2.</p>")>]
let ``CanvasAssignmentResource.ofCanvasResource can parse assignment content.``(path: string) (title: string) (content: string) =

    // arrange
    use imsccFile = new ImsccFile(path)
    let manifest = ImsccManifest.read imsccFile
    let canvasResources = manifest |> TestHelpers.getCanvasResourcesOfType CanvasResourceType.Assignment

    // act
    let assignmentResources =
        canvasResources
        |> List.map(Option.map (CanvasAssignmentResource.ofCanvasResource manifest))

    // assert
    let isMatch (pr: CanvasAssignmentResource option) = pr.IsSome && pr.Value.Title = title
    Assert.True(assignmentResources |> Seq.forall(Option.isSome))
    Assert.True(assignmentResources |> Seq.exists(isMatch))
    let assignment = assignmentResources |> Seq.find(isMatch)
    Assert.Equal(content, assignment.Value.Content)