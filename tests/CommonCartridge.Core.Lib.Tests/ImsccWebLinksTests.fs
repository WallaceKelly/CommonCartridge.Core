module ImsccWebLinksTests

open Xunit
open CommonCartridge

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink1)>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink2)>]
let ``ImsccWebLinks.Schema.Load loads the weblinks file without throwing``(path: string) =
    // arrange, act assert
    ImsccWebLink.Schema.Load(path)
    |> ignore

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink1, "External URL 1")>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink2, "External URL 2")>]
let ``ImsccWebLinks.Schema.Load reads the weblinks title``(path: string, expectedTitle: string) =
    // arrange, act
    let webLink = ImsccWebLink.Schema.Load(path)
    // assert
    Assert.Equal(expectedTitle, webLink.Title)

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink1, "https://www.google.com")>]
[<InlineData(ImsccTestFilePaths.ModulesWebLink2, "https://www.bing.com")>]
let ``ImsccWebLinks.Schema.Load reads the weblinks URL``(path: string, expectedUrl: string) =
    // arrange, act
    let webLink = ImsccWebLink.Schema.Load(path)
    // assert
    Assert.Equal(expectedUrl, webLink.Url.Href)