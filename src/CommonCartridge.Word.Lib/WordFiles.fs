namespace CommonCartridge.Word

open System
open System.IO
open NPOI.XWPF.UserModel

type ImsccWordFile =
    { Path: string
      Document: XWPFDocument }

module ImsccWordFile =

    let private createWordFile (path: string) =
        let doc = XWPFDocument()
        use fs = new FileStream(path, FileMode.Create, FileAccess.Write)
        doc.Write(fs)
        doc

    let private createInternal (overwrite: bool) (path: string) =

        if (not overwrite && File.Exists(path)) then
            sprintf "The file '%s' already exists." path
            |> ArgumentException
            |> raise

        if (File.Exists(path)) then
            File.Delete(path)

        let doc = createWordFile path

        { Path = path; Document = doc }

    let createNew = createInternal false
    let create = createInternal true

    let openFile (path: string) =
        use fs = new FileStream(path, FileMode.Open, FileAccess.Read)
        let doc = XWPFDocument(fs)
        { Path = path; Document = doc }

    let saveFile (wordFile: ImsccWordFile) =
        use fs = new FileStream(wordFile.Path, FileMode.Create, FileAccess.Write)
        wordFile.Document.Write(fs)

    let delete (doc: ImsccWordFile) =
        if (File.Exists(doc.Path)) then
            File.Delete(doc.Path)

