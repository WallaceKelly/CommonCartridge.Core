module CommandLineArgsTests

open System.IO
open System.Reflection
open Xunit
open CommonCartridge.Core.Cmd.CommandLineArgs
open CommonCartridge.Core.Cmd

let private getFullPath (filename: string) =
    let getParent (p: string) = Path.GetDirectoryName(p)
    let basePath =
        Assembly.GetExecutingAssembly().Location
        |> getParent
        |> getParent
        |> getParent
        |> getParent
        |> getParent
        |> getParent
    Path.Combine(basePath, filename)

let private split (s: string) = s.Split()

let private emptyImsccPath = getFullPath @"imscc-files\empty.imscc"
let private unitTestingWordPath = getFullPath @"docx-files\unit-testing.docx"

[<Fact>]
let ``Parses conversion from Imscc file`` () =
    // arrange
    let args = sprintf "%s" emptyImsccPath |> split
    // act
    let result = parse(args)
    // assert
    Assert.True(ConvertCommand.isConvertToWord result)
    let parms = match result with ConvertToWord p -> p | _ -> failwith "Conversion failure"
    Assert.Equal(emptyImsccPath, parms.ImsccFilePath)
    Assert.EndsWith("empty.docx", parms.WordFilePath)

[<Fact>]
let ``Parses conversion from Imscc file to Word file`` () =
    // arrange
    let args = sprintf "%s -o result.docx" emptyImsccPath |> split
    // act
    let result = parse(args)
    // assert
    Assert.True(ConvertCommand.isConvertToWord result)
    let parms = match result with ConvertToWord p -> p | _ -> failwith "Conversion failure"
    Assert.Equal(emptyImsccPath, parms.ImsccFilePath)
    Assert.EndsWith("result.docx", parms.WordFilePath)

[<Fact(Skip="Not supported yet")>]
let ``Parses conversion from Word file`` () =
    // arrange
    let args = sprintf "%s" unitTestingWordPath |> split
    // act
    let result = parse(args)
    // assert
    Assert.True(ConvertCommand.isConvertToImscc result)
    let parms = match result with ConvertToImscc p -> p | _ -> failwith "Conversion failure"
    Assert.Equal(unitTestingWordPath, parms.WordFilePath)
    Assert.EndsWith("unit-testing.imscc", parms.ImsccFilePath)

[<Fact(Skip="Not supported yet")>]
let ``Parses conversion from Word file to Imscc file`` () =
    // arrange
    let args = sprintf "%s -o result.imscc" unitTestingWordPath |> split
    // act
    let result = parse(args)
    // assert
    Assert.True(ConvertCommand.isConvertToImscc result)
    let parms = match result with ConvertToImscc p -> p | _ -> failwith "Conversion failure"
    Assert.Equal(unitTestingWordPath, parms.WordFilePath)
    Assert.EndsWith("unit-testing.imscc", parms.ImsccFilePath)
