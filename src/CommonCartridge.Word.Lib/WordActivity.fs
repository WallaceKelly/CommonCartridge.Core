module CommonCartridge.Word.WordActivity

open CommonCartridge

let append (wordFile: ImsccWordFile) (item: ImsccModuleItem) =
    let p = wordFile.Document.CreateParagraph()
    let r = p.CreateRun()
    r.SetText(sprintf "%A: %s" item.Type item.ModuleTitle)

let appendAll (wordFile: ImsccWordFile) = Seq.iter(append wordFile)

