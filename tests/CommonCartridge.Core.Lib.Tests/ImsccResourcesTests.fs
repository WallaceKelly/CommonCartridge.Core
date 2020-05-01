module ImsccResourceTests

open Xunit
open CommonCartridge

[<Theory>]
[<InlineData("Assignment 1", ImsccResourceType.Assignment)>]
[<InlineData("Assignment 2", ImsccResourceType.Assignment)>]
[<InlineData("Discussion 1", ImsccResourceType.Discussion)>]
[<InlineData("Discussion 2", ImsccResourceType.Discussion)>]
[<InlineData("External URL 1", ImsccResourceType.External)>]
[<InlineData("External URL 2", ImsccResourceType.External)>]
// These don't have resources apparently?
// [<InlineData("External Tool 1", ImsccResourceType.External)>]
// [<InlineData("External Tool 2", ImsccResourceType.External)>]
[<InlineData("file 1.txt", ImsccResourceType.File)>]
[<InlineData("file 2.txt", ImsccResourceType.File)>]
[<InlineData("Page 1", ImsccResourceType.Page)>]
[<InlineData("Page 2", ImsccResourceType.Page)>]
[<InlineData("Quiz 1", ImsccResourceType.Quiz)>]
[<InlineData("Quiz 2", ImsccResourceType.Quiz)>]
let ``ImsccResourceType.getResourceType can recognize activity types`` (title: string) (resourceType: ImsccResourceType) =
    // arrange
    use imsccFile = new ImsccFile(ImsccTestFilePaths.Modules)
    let modul =
        imsccFile
        |> ImsccModule.fromFile 
        |> Seq.collect(fun m -> m.Items)
        |> Seq.where(fun m -> m.ModuleTitle = title)
        |> Seq.exactlyOne
    // act
    let resource =
        imsccFile
        |> ImsccResource.fromFile
        |> Seq.where(fun r -> r.Identifier = modul.IdentifierRef)
        |> Seq.exactlyOne 
    // assert
    Assert.Equal(resourceType, resource.Type)

