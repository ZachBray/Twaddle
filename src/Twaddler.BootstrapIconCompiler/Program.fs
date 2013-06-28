open System.IO

let input = __SOURCE_DIRECTORY__ + @"\..\..\data\bootstrap\icons.txt"
let output = __SOURCE_DIRECTORY__ + @"\..\Twaddler\Bootstrap.Icons.fs"

let compile() =
    let text = File.ReadAllText input
    let lines =
        text.Split [|' '|]
        |> Seq.map (fun icon -> icon.Trim())
        |> Seq.map (fun icon ->
            icon.Split[|'-'|] |> Seq.skip 1 |> Seq.map (fun word ->
                word.Substring(0, 1).ToUpper() + word.Substring(1))
            |> String.concat "",
            icon)

    use file = new StreamWriter(output)

    fprintfn file "module Bootstrap.Icons"
    fprintfn file ""
    fprintfn file "open DOM"
    fprintfn file "open Html"
    fprintfn file ""

    for name, icon in lines do
        fprintfn file ""
        fprintfn file "let %s = i |> addClass \"%s\"" name icon

    file.Close()

compile()