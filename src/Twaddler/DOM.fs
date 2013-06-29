module DOM

type Content =
    | Children of Tag list
    | Raw of string
    | CannotHaveContent

and Tag = 
    {
        Name : string
        Attributes : Map<string, string>
        Content : Content
    }

module Html =

    let tag name =
        {
            Name = name
            Attributes = Map.empty
            Content = Children []
        }

    let div = tag "div"
    let a = tag "a"
    let button = tag "button"
    let script = tag "script"
    let span = tag "span"
    let title name = { tag "title" with Content = Raw name }
    let link = { tag "link" with Content = CannotHaveContent }
    let meta = { tag "meta" with Content = CannotHaveContent }
    let p = tag "p"
    let h n = tag (sprintf "h%i" n)
    let br = { tag "br" with Content = CannotHaveContent }
    let i = tag "i"

let addKids children (parent : Tag) =
    match parent.Content with
    | CannotHaveContent -> failwith "%s tags cannnot have content." parent.Name
    | Raw str -> { parent with Content = Children({ Html.span with Content = Raw str } :: children) }
    | Children xs -> { parent with Content = Children(children @ xs) }

let addRaw str (parent : Tag) =
    match parent.Content with
    | Children [] -> { parent with Content = Raw str }
    | _ -> parent |> addKids [{ Html.span with Content = Raw str }]

let addClass value (parent : Tag) =
    { parent with 
        Attributes = 
            match parent.Attributes.TryFind "class" with
            | None -> parent.Attributes |> Map.add "class" value
            | Some oldValue -> parent.Attributes |> Map.add "class" (sprintf "%s %s" oldValue value) }

let addAttr name value (parent : Tag) =
    { parent with Attributes = parent.Attributes |> Map.add name value }

let addId value parent =
    parent |> addAttr "id" value

let addAttrs nameValues parent =
    nameValues |> List.fold (fun acc (name, value) ->
        acc |> addAttr name value) parent

let (<==) x y = x, y

type HtmlDoc =
    {
        Path : string
        Header : Tag list
        Body : Tag list
    }


module Compiler =

    open System.IO

    let indent xs = xs |> Seq.map (fun x -> "  " + x)
    
    let compileAttributes xs =
        xs |> Seq.map (fun (KeyValue(k, v)) -> sprintf " %s=\"%s\"" k v)
        |> String.concat ""

    let rec compileTag (tag : Tag) =
        let attributes = tag.Attributes |> compileAttributes
        seq {
            match tag.Content with
            | CannotHaveContent -> 
                yield sprintf "<%s%s>" tag.Name attributes
            | Children [] -> 
                yield sprintf "<%s%s></%s>" tag.Name attributes tag.Name
            | Raw str -> 
                yield sprintf "<%s%s>%s</%s>" tag.Name attributes str tag.Name
            | Children tags -> 
                yield sprintf "<%s%s>" tag.Name attributes 
                yield! tags |> Seq.collect compileTag |> indent
                yield sprintf "</%s>" tag.Name
        }

    let compileDoc (doc : HtmlDoc) =
        seq {
            yield "<!DOCTYPE html>"
            yield "<html>"
            yield! Html.tag "header" |> addKids doc.Header |> compileTag |> indent
            yield! Html.tag "body" |> addKids doc.Body |> compileTag |> indent
            yield "</html>"
        }

    let compile root doc =
        let lines = compileDoc doc
        let fullPath = Path.Combine(root, doc.Path)
        File.WriteAllLines(fullPath, lines |> Seq.toArray)