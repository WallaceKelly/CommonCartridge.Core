module ImsccManifestsTests

open Xunit
open CommonCartridge
open CommonCartridge.Canvas

[<Theory>]
[<InlineData(ImsccTestFilePaths.EmptyManifest)>]
[<InlineData(ImsccTestFilePaths.ModulesManifest)>]
let ``Schema.Load loads the manifest without throwing``(path: string) =
    // arrange, act assert
    ImsccManifest.Schema.Load(path)
    |> ignore

[<Theory>]
[<InlineData(ImsccTestFilePaths.EmptyManifest)>]
[<InlineData(ImsccTestFilePaths.ModulesManifest)>]
let ``Schema.Load loads empty manifest as identical XML``(path: string) =
    // arrange, act assert
    ImsccManifest.Schema.Load(path).XElement
    |> TestHelpers.savesIdenticalXml path
    |> Assert.True

[<Fact>]
let ``Schema.Load reads module titles from manifest file``() =
    // arrange, act
    let titles = 
        ImsccManifest.Schema.Load(ImsccTestFilePaths.ModulesManifest).Manifest.Value.Organization
        |> Option.map(fun o -> o.Item.Items)
        |> Option.defaultValue [||]
        |> Seq.map(fun i -> i.Title)
    // assert
    Assert.NotEmpty(titles)
    Assert.Equal(2, titles |> Seq.length)
    Assert.Equal("Module 1", titles |> Seq.item 0)
    Assert.Equal("Module 2", titles |> Seq.item 1)

[<Fact>]
let ``Schema.Load reads activity titles from manifest file``() =
    // arrange, act
    let titles = 
        ImsccManifest.Schema.Load(ImsccTestFilePaths.ModulesManifest).Manifest.Value.Organization
        |> Option.map(fun o -> o.Item.Items)
        |> Option.defaultValue [||]
        |> Seq.collect(fun m -> m.Items)
        |> Seq.map(fun i -> i.Title)
    // assert
    Assert.NotEmpty(titles)
    Assert.Equal(16, titles |> Seq.length)
    Assert.Equal("Assignment 1", titles |> Seq.item 0)
    Assert.Equal("Quiz 1", titles |> Seq.item 1)

