module CommonCartridge.Word.WordActivity

open CommonCartridge

let append (wordFile: ImsccWordFile) (item: ImsccModuleItem) =
    let p = wordFile.Document.CreateParagraph()
    let r = p.CreateRun()
    r.SetText(item.Title)

let appendAll (wordFile: ImsccWordFile) = Seq.iter(append wordFile)

