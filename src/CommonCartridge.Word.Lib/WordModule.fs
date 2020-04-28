module CommonCartridge.Word.WordModule

open CommonCartridge

let append (wordFile: ImsccWordFile) (modul: ImsccModule) =
    let p = wordFile.Document.CreateParagraph()
    let r = p.CreateRun()
    r.IsBold <- true
    r.SetText(modul.Title)
    modul.Items |> WordActivity.appendAll wordFile

let appendAll (wordFile: ImsccWordFile) = Seq.iter(append wordFile)
