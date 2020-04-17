namespace CommonCartridge.Core.Cmd

open System
open System.IO
open Argu
open CommonCartridge.Core.Cmd.ActivePatterns

type CliArguments =
    | [<Mandatory;MainCommand;First;ExactlyOnce>] Input of path:string
    | [<AltCommandLine("-o")>] Output of path:string

    with
        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Input _ ->   "input file to convert."
                | Output _ ->   "output folder or file path."

type ConvertParams =
    { ImsccFilePath: string 
      WordFilePath: string }

type ConvertCommand =
    | ConvertToWord of parms:ConvertParams
    | ConvertToImscc of parms:ConvertParams

module internal ConvertCommand =
    let isConvertToWord c = match c with ConvertToWord(_) -> true | _ -> false
    let isConvertToImscc c = match c with ConvertToImscc(_) -> true | _ -> false
    let create (inputPath: string) (outputPath: string) =
        match inputPath with
        | ImsccFile _ ->
            match outputPath with
            | WordFile _ -> Some(ConvertToWord { ImsccFilePath = inputPath; WordFilePath = outputPath })
            | _ -> None
        | _ -> None

module CommandLineArgs =

    let parse (argv: string[]) =

        let colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red
        let errorHandler = ProcessExiter(colorizer=colorizer)
        let parser = ArgumentParser.Create<CliArguments>(programName="imscc-convert.exe", errorHandler=errorHandler)
        let args = parser.Parse(argv)

        let parseInputPath path =
            match path with
            | Unspecified _ -> failwith "Input file was not specified"
            | Unrecognized ext -> failwithf "Input file of type '*%s' is not recognized" ext
            | NotFound p -> failwithf "Input file '%s' could not be found" p
            | Existing p -> p

        let inputPath = args.PostProcessResult(<@ CliArguments.Input @>, parseInputPath)

        let expectedOutputPathExt =
            match inputPath with
            | WordFile _ -> FileExtensionImscc
            | ImsccFile _ -> FileExtensionWord
            | UnrecognizedFile _ -> failwith "Should not reach this point: {ADCF49BF-78E8-4FE9-9B2A-20FCE1AB1A9F}"
    
        let parseOutputPath path =
            let outputPath =
                match path with
                | Unspecified _ -> failwith "Should not reach this point: {FD4F2193-EA43-4DEC-B68E-EB80062497B6}"
                | Unrecognized ext -> failwithf "Output file of type '*%s' is not recognized" ext
                | Existing p -> p
                | NotFound p -> p
            match ConvertCommand.create inputPath outputPath with
            | Some _ -> outputPath
            | None ->
                failwithf "Conversion from '*%s' to '*%s' is not supported."
                    (Path.GetExtension(inputPath))
                    (Path.GetExtension(outputPath))

        let outputPath = 
            match args.TryGetResult(<@ CliArguments.Output @>) with
            | None -> Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(inputPath)), expectedOutputPathExt)
            | Some _ -> args.PostProcessResult(<@ CliArguments.Output @>, parseOutputPath)

        match ConvertCommand.create inputPath outputPath with
        | Some cmd -> cmd
        | None -> failwith "Should not reach this point: {86889932-1C85-4B90-8D53-9F8F925E4C7A}"
