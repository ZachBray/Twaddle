open System.IO
open System.Web.Script.Serialization

let root = __SOURCE_DIRECTORY__ + @"\..\..\data"

let buildDictionary() =
    let extensions = [|"adj"; "adv"; "noun"; "verb"|]
    let files = extensions |> Array.map (sprintf @"%s\wordnet\data.%s" root)
    let offset = 29
    let lines = files |> Seq.collect (File.ReadLines >> Seq.skip offset)

    let parseData (data : string) =
        match data.Split [|' '|] |> Array.toList with
        | _::_::wordType::_::name::_ -> Some(wordType, name)
        | _ -> None

    let serializer = JavaScriptSerializer()

    let parseLine (line : string) =
        match line.Split [|'|'|] with
        | [|data; definition|] -> 
            let shortenedDefinition =
                definition.Trim().Split([|';'|]).[0]
            parseData data |> Option.map (fun (wordType, name) -> name, wordType, serializer.Serialize shortenedDefinition)
        | _ -> None


    printf "Writing dictionary..."
    let outputDir = sprintf @"%s\dictionaries" root
    if not (Directory.Exists outputDir) then
        Directory.CreateDirectory outputDir
        |> ignore
    let outputPath = sprintf @"%s\english.js" outputDir
    if File.Exists outputPath then
        File.Delete outputPath
    use file = new StreamWriter(outputPath)
    fprintfn file "var words = ["
    let definitions = 
        lines 
        |> Seq.choose parseLine 
        |> Seq.choose (fun (word, t, def) ->
            let word = word.Replace("_", " ")
            let words = word.Split[|' '|]
            let isRecursive = words |> Array.exists def.Contains
            if isRecursive then None
            else Some (word, t, def))
        |> Seq.cache
    for word, t, def in definitions do
        fprintfn file "\"%s\"," word
    fprintfn file "];"
    fprintfn file "var definitions = ["
    for word, t, def in definitions do
        fprintfn file "%s," def
    fprintfn file "];"
    file.Close()
    printfn "\ndone!"
buildDictionary()