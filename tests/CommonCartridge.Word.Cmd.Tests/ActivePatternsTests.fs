module ActivePatternsTests

open System.IO
open Xunit
open CommonCartridge.Core.Cmd.ActivePatterns

[<Fact>]
let ``Correctly identifies Missing file path`` () =
    // arrange, act
    let result =
        match "" with
        | Missing -> true
        | _ -> false
    // assert
    Assert.True(result)

[<Fact>]
let ``Correctly identifies FilePaths`` () =
    // arrange, act
    let result =
        match "test.txt" with
        | FilePath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)

[<Fact>]
let ``Expands FilePaths to full paths`` () =
    // arrange, act
    let result =
        match "test.txt" with
        | FilePath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.True(Path.IsPathRooted(result))
    Assert.Contains("test.txt", result)

[<Fact>]
let ``Correctly identifies FolderPaths`` () =
    // arrange, act
    let result =
        match "test" with
        | FolderPath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)

[<Fact>]
let ``Expands FolderPaths to full paths`` () =
    // arrange, act
    let result =
        match "test" with
        | FolderPath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.True(Path.IsPathRooted(result))
    Assert.EndsWith("test", result)

[<Fact>]
let ``Correctly identifies relative folder paths`` () =
    // arrange, act
    let result =
        match "../" with
        | FolderPath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)

[<Fact>]
let ``Expands relative FolderPaths to full paths`` () =
    // arrange, act
    let result =
        match "../" with
        | FolderPath p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.True(Path.IsPathRooted(result))

[<Theory>]
[<InlineData("test.docx")>]
[<InlineData("test.DOCX")>]
[<InlineData(@"C:\temp\test.docx")>]
let ``Recognizes Word file extensions`` (path: string) =
    // arrange, act
    let result =
        match path with
        | WordFile p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.Contains(path.ToLower(), result.ToLower())

[<Theory>]
[<InlineData("test.imscc")>]
[<InlineData("test.IMSCC")>]
[<InlineData(@"C:\temp\test.imscc")>]
let ``Recognizes Common Cartridge file extensions`` (path: string) =
    // arrange, act
    let result =
        match path with
        | ImsccFile p -> p
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.Contains(path.ToLower(), result.ToLower())

[<Theory>]
[<InlineData("test.txt")>]
[<InlineData(@"C:\temp\test.txt")>]
let ``Recognizes unrecognized file extensions`` (path: string) =
    // arrange, act
    let result =
        match path with
        | UnrecognizedFile ext -> ext
        | _ -> ""
    // assert
    Assert.NotEmpty(result)
    Assert.Equal(".txt", result.ToLower())

[<Theory>]
[<InlineData("test")>]
[<InlineData(@"C:\temp\test")>]
let ``Recognizes missing file extensions`` (path: string) =
    // arrange, act
    let result =
        match path with
        | UnrecognizedFile ext -> ext
        | _ -> "txt"
    // assert
    Assert.Empty(result)
