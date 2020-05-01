module ImsccModulesTests

open Xunit
open CommonCartridge

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``ImsccModule.fromManifest can read modules without throwing`` (path: string) =
    // arrange
    use imsccFile = new ImsccFile(path)
    // act
    // assert
    imsccFile |> ImsccModule.fromFile |> ignore

[<Theory>]
[<InlineData("Assignment 1", ImsccResourceType.Assignment)>]
[<InlineData("Assignment 2", ImsccResourceType.Assignment)>]
[<InlineData("Discussion 1", ImsccResourceType.Discussion)>]
[<InlineData("Discussion 2", ImsccResourceType.Discussion)>]
[<InlineData("External URL 1", ImsccResourceType.External)>]
[<InlineData("External URL 2", ImsccResourceType.External)>]
[<InlineData("file 1.txt", ImsccResourceType.File)>]
[<InlineData("file 2.txt", ImsccResourceType.File)>]
[<InlineData("Page 1", ImsccResourceType.Page)>]
[<InlineData("Page 2", ImsccResourceType.Page)>]
[<InlineData("Quiz 1", ImsccResourceType.Quiz)>]
[<InlineData("Quiz 2", ImsccResourceType.Quiz)>]
let ``ImsccModule.fromManifest can read activity titles`` (title: string) (resourceType: ImsccResourceType) =
    // arrange
    use imsccFile = new ImsccFile(ImsccTestFilePaths.Modules)
    // act
    imsccFile
    |> ImsccModule.fromFile
    |> Seq.collect(fun m -> m.Items)
    |> Seq.where(fun i -> i.ModuleTitle = title)
    |> Seq.exactlyOne // assert
    |> ignore
