module WordFilesTests

open System
open System.IO
open Xunit
open CommonCartridge.Word

[<Fact>]
let ``WordFiles.createNew creates an empty Word file`` () =
    // arrange
    let path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
    // act
    let wordFile = ImsccWordFile.createNew path
    try
        // assert
        Assert.True(File.Exists(wordFile.Path))
        Assert.True(FileInfo(wordFile.Path).Length > 0L)
    finally
        File.Delete(path)

[<Fact>]
let ``WordFiles.openFile opens an existing Word file`` () =
    // arrange
    let path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
    try
        ImsccWordFile.createNew path |> ignore
        // act
        let wordFile = ImsccWordFile.openFile path
        // assert
        Assert.NotEmpty(wordFile.Path)
        Assert.NotNull(wordFile.Document)
    finally
        File.Delete(path)

[<Fact>]
let ``WordFiles.saveFile saves an existing Word file`` () =
    // arrange
    let path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
    try
        ImsccWordFile.createNew path |> ignore
        let wordFile = ImsccWordFile.openFile path
        // act
        wordFile |> ImsccWordFile.saveFile
        // assert
        Assert.True(File.Exists(wordFile.Path))
    finally
        File.Delete(path)

[<Fact>]
let ``WordFiles.createNew does not replace existing file when creating`` () =
    // arrange
    let path = Path.GetTempFileName()
    try
        // act, assert
        let ex = Assert.Throws<ArgumentException>(fun _ ->
            ImsccWordFile.createNew path |> ignore
        )
        Assert.Contains("already exists", ex.Message)
        Assert.Contains(path, ex.Message)
    finally
        File.Delete(path)

[<Fact>]
let ``WordFiles.create replaces existing file when creating`` () =
    // arrange
    let path = Path.GetTempFileName()
    // act
    let wordFile = ImsccWordFile.create path
    try
        // assert
        Assert.True(File.Exists(wordFile.Path))
    finally
        File.Delete(path)

[<Fact>]
let ``WordFiles.delete deletes a Word file`` () =
    // arrange
    let path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
    let wordFile = ImsccWordFile.createNew path
    // act
    wordFile |> ImsccWordFile.delete
    // assert
    Assert.False(File.Exists(wordFile.Path))

