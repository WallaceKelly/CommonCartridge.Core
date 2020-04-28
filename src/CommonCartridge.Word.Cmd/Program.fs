// Learn more about F# at http://fsharp.org

open System
open Argu
open System.Reflection
open System.IO
open CommonCartridge.Core.Cmd
open CommonCartridge
open CommonCartridge.Word

let convertToWord (dst: string) (src: string) =
    
    let imsccFile = ImsccFile.load src
    try
        let wordFile = ImsccWordFile.create dst
        let modules = ImsccModule.fromFile imsccFile
        // TODO: make resources a member of modules
        // let resources = ImsccResource.fromFile imsccFile
    
        modules |> WordModule.appendAll wordFile
        wordFile |> ImsccWordFile.saveFile
    finally
        imsccFile |> ImsccFile.unload

[<EntryPoint>]
let main argv =

    let command = CommandLineArgs.parse argv

    match command with
    | ConvertToWord c -> c.ImsccFilePath |> convertToWord c.WordFilePath
    | ConvertToImscc c -> failwith "Not supported yet"

    0 // return an integer exit code
