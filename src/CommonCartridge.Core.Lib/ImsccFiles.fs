namespace CommonCartridge

open System.IO
open System.IO.Compression

type ImsccFile(path: string) =
    
    let extractedFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())

    do
        ZipFile.ExtractToDirectory(path, extractedFolder)

    member val Path = path
    member val ExtractedFolder = extractedFolder

    interface System.IDisposable with
        member x.Dispose() =
            try
                Directory.Delete(x.ExtractedFolder, true)
            with _ -> ()