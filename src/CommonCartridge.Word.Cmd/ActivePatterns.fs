module internal CommonCartridge.Core.Cmd.ActivePatterns

open System
open System.IO

let FileExtensionImscc = ".imscc"
let FileExtensionWord = ".docx"

let (|FilePath|FolderPath|Missing|) path =
    if(String.IsNullOrWhiteSpace(path)) then Missing
    else
        let fullPath = Path.GetFullPath(path)
        if (String.IsNullOrWhiteSpace(Path.GetExtension(fullPath))) then (FolderPath fullPath)
        else (FilePath fullPath)

let (|WordFile|ImsccFile|UnrecognizedFile|) (filePath: string) =
    let extension = Path.GetExtension(filePath)
    match extension.ToLower() with
    | ext when ext = FileExtensionWord -> WordFile filePath
    | ext when ext = FileExtensionImscc -> ImsccFile filePath
    | ext -> UnrecognizedFile ext

let (|Existing|NotFound|Unspecified|Unrecognized|) path =
    match path with
    | Missing -> Unspecified ""
    | FolderPath p -> Unspecified p
    | FilePath filePath ->
        match File.Exists(filePath) with
        | false -> NotFound filePath
        | true ->
            match filePath with
            | WordFile p -> Existing p
            | ImsccFile p -> Existing p
            | UnrecognizedFile ext -> Unrecognized ext

