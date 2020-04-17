module ImsccFilesTests

open Xunit
open CommonCartridge
open System.IO

[<Theory>]
[<InlineData(ImsccFilePaths.Empty)>]
[<InlineData(ImsccFilePaths.Modules)>]
let ``ImsccFiles.load and ImsccFiles.unload do not throw`` (path: string) =
    // arrange, act assert
    path
    |> ImsccFile.load
    |> ImsccFile.unload

[<Theory>]
[<InlineData(ImsccFilePaths.Empty)>]
[<InlineData(ImsccFilePaths.Modules)>]
let ``ImsccFiles.unload deletes extracted IMSCC files after unload`` (path: string) =
    // arrange
    let imscc = path |> ImsccFile.load
    Assert.True(Directory.Exists(imscc.ExtractedFolder))
    // act
    imscc |> ImsccFile.unload
    // assert
    Assert.False(Directory.Exists(imscc.ExtractedFolder))

[<Theory>]
[<InlineData(ImsccFilePaths.Empty)>]
[<InlineData(ImsccFilePaths.Modules)>]
let ``ImsccFiles.load extracts manifest file from IMSCC files`` (path: string) =
    // arrange
    let imscc = path |> ImsccFile.load
    try
        // act
        let manifestFilePath = Path.Combine(imscc.ExtractedFolder, "imsmanifest.xml")
        // assert
        Assert.True(File.Exists(manifestFilePath))
    finally
        imscc |> ImsccFile.unload

[<Theory>]
[<InlineData(ImsccFilePaths.Empty)>]
[<InlineData(ImsccFilePaths.Modules)>]
let ``ImsccFiles.load extracts course_settings folder from IMSCC files`` (path: string) =
    // arrange
    let imscc = path |> ImsccFile.load
    try
        // act
        let courseSettingsFolder = Path.Combine(imscc.ExtractedFolder, "course_settings")
        // assert
        Assert.True(Directory.Exists(courseSettingsFolder))
    finally
        imscc |> ImsccFile.unload
