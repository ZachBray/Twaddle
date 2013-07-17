[<FunScript.JS>]
module DOM

type Content =
    | Children of Tag[]
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
            Content = Children [||]
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
    let h n = tag ("h" + n.ToString())
    let br = { tag "br" with Content = CannotHaveContent }
    let i = tag "i"

let addKids children (parent : Tag) =
    match parent.Content with
    | CannotHaveContent -> failwith "%s tags cannnot have content." parent.Name
    | Raw str -> { parent with Content = Children(Array.append [|{ Html.span with Content = Raw str }|] children) }
    | Children xs -> { parent with Content = Children(Array.append children xs) }

let addRaw str (parent : Tag) =
    match parent.Content with
    | Children xs when xs.Length = 0 -> { parent with Content = Raw str }
    | _ -> parent |> addKids [|{ Html.span with Content = Raw str }|]

let addClass value (parent : Tag) =
    { parent with 
        Attributes = 
            match parent.Attributes.TryFind "class" with
            | None -> parent.Attributes |> Map.add "class" value
            | Some oldValue -> parent.Attributes |> Map.add "class" (oldValue + " " + value) }

let addAttr name value (parent : Tag) =
    { parent with Attributes = parent.Attributes |> Map.add name value }

let addId f parent =
    parent |> addAttr "id" (f())

let addAttrs nameValues parent =
    nameValues |> Array.fold (fun acc (name, value) ->
        acc |> addAttr name value) parent

let (<==) x y = x, y

type HtmlDoc =
    {
        Header : Tag[]
        Body : Tag[]
    }


module Compiler =

    let indent xs = xs |> Seq.map (fun x -> "  " + x)
    
    let compileAttributes xs =
        xs |> Seq.map (fun (k, v) -> " " + k + "=\"" + v + "\"")
        |> String.concat ""

    
    let openTag (tag : Tag) =
        let attributes = tag.Attributes |> Map.toSeq |> compileAttributes
        "<" + tag.Name + attributes + ">"

    let closingTag (tag : Tag) = 
        "</" + tag.Name + ">"

    let rec compileTag (tag : Tag) =
        seq {
            match tag.Content with
            | CannotHaveContent -> 
                yield openTag tag
            | Raw str -> 
                yield openTag tag + str + closingTag tag
            | Children tags ->
                yield openTag tag 
                yield! tags |> Seq.collect compileTag |> indent
                yield closingTag tag
        }

    let compileDoc (doc : HtmlDoc) =
        seq {
            yield "<!DOCTYPE html>"
            yield "<html>"
            yield! Html.tag "header" |> addKids doc.Header |> compileTag |> indent
            yield! Html.tag "body" |> addKids doc.Body |> compileTag |> indent
            yield "</html>"
        }