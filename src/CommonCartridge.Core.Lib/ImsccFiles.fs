namespace CommonCartridge

open System.IO
open System.IO.Compression

[<CLIMutable>]
type ImsccFile = {
    Path: string
    ExtractedFolder: string
}

module ImsccFile =

    let load(path: string) = 
    
        let extractedFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        ZipFile.ExtractToDirectory(path, extractedFolder)

        { Path = path
          ExtractedFolder = extractedFolder }

    let unload(imscc: ImsccFile) =
        Directory.Delete(imscc.ExtractedFolder, true)