module internal CommonCartridge.Canvas.CanvasHtmlParsing

open System.Text.RegularExpressions

// in general, parsing HTML with regex is a bad idea.
// in this case, the Canvas-generated HTML is known to be well-formed.

let getTitle(html: string) =
    let options = RegexOptions.IgnoreCase ||| RegexOptions.Singleline
    let matches = Regex.Match(html, "<title>(.*?)</title>", options)
    match matches.Success with
    | false -> "Unknown title"
    | true -> matches.Groups.[1].Value.Trim()
        

let getBody(html: string) =
    let options = RegexOptions.IgnoreCase ||| RegexOptions.Singleline
    let matches = Regex.Match(html, "<body>(.*?)</body>", options)
    match matches.Success with
    | false -> "Unknown body"
    | true -> matches.Groups.[1].Value.Trim()
