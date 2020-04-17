module CommonCartridge.Word.WordModule

open CommonCartridge

let append (wordFile: ImsccWordFile) (modul: ImsccModule) =
    let p = wordFile.Document.CreateParagraph()
    let r = p.CreateRun()
    r.SetText(modul.Title)

let appendAll (wordFile: ImsccWordFile) = Seq.iter(append wordFile)
