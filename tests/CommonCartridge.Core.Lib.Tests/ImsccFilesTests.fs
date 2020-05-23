module ImsccFilesTests

open Xunit
open CommonCartridge
open System.IO

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``ImsccFiles.Dispose and ImsccFiles.unload do not throw`` (path: string) =
    // arrange, act, assert
    use imsccFile = new ImsccFile(path)
    (
    )

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``ImsccFile.Dispose deletes extracted IMSCC files after unload`` (path: string) =
    // arrange
    let mutable extractedFolder = ""
    using (new ImsccFile(path)) (fun imsccFile ->
        extractedFolder <- imsccFile.ExtractedFolder
        Assert.True(Directory.Exists(imsccFile.ExtractedFolder))
    ) // act
    // assert
    Assert.False(Directory.Exists(extractedFolder))

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``ImsccFile.ctor extracts manifest file from IMSCC files`` (path: string) =
    // arrange
    use imsccFile = new ImsccFile(path)
    // act
    let manifestFilePath = Path.Combine(imsccFile.ExtractedFolder, "imsmanifest.xml")
    // assert
    Assert.True(File.Exists(manifestFilePath))

[<Theory>]
[<InlineData(ImsccTestFilePaths.Empty)>]
[<InlineData(ImsccTestFilePaths.Modules)>]
let ``ImsccFile.ctor extracts course_settings folder from IMSCC files`` (path: string) =
    // arrange
    use imsccFile = new ImsccFile(path)
    // act
    let courseSettingsFolder = Path.Combine(imsccFile.ExtractedFolder, "course_settings")
    // assert
    Assert.True(Directory.Exists(courseSettingsFolder))
