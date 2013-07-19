namespace Bootstrap

open DOM
open Html
open FunScript

[<JS>]
module Document =
    let create name head content =
        {
            Header = 
                [|
                    yield title name
                    yield meta |> addAttrs [|
                        "name" <== "viewport"
                        "content" <== "width=device-width, initial-scale=1.0"
                    |]
                    yield link |> addAttrs [|
                        "href" <== "css/bootstrap.min.css" 
                        "rel" <== "stylesheet" 
                        "media" <== "screen"
                    |]
                    yield link |> addAttrs [|
                        "href" <== "css/bootstrap-responsive.min.css" 
                        "rel" <== "stylesheet" 
                        "media" <== "screen"
                    |]
//                    yield link |> addAttrs [|
//                        "href" <== "css/bootstrap-overrides.css" 
//                        "rel" <== "stylesheet" 
//                        "media" <== "screen"
//                    |]
                    yield! head
                |]
            Body =
                [|
                    yield script |> addAttrs [|
                        "src" <== "js/jquery.min.js"
                    |]
                    yield script |> addAttrs [|
                        "src" <== "js/bootstrap.min.js"
                    |]
                    yield div 
                          |> addClass "container"
                          |> addKids content
                |]
        }
        
[<JS>]
module Container =
    let row = div |> addClass "row"
    let span n = div |> addClass ("span" + n.ToString())
    let offsetSpan i n = span n |> addClass ("offset" + i.ToString())
    let hero = div |> addClass "hero-unit"
    let table = table |> addClass "table"

[<JS>]
module Table =
    module Classes =
        let bordered = "table-bordered"
    
[<JS>]
module Text =
    let left p = p |> addClass "text-left" 
    let center p = p |> addClass "text-center" 
    let right p = p |> addClass "text-right" 
    
[<JS>]
module Button =
    let normal = a |> addClass "btn"
    let primary = normal |> addClass "btn-primary"
    let info = normal |> addClass "btn-info"
    let success = normal |> addClass "btn-success"
    let warning = normal |> addClass "btn-warning"
    let danger = normal |> addClass "btn-danger"
    let inverse = normal |> addClass "btn-inverse"
    let link = normal |> addClass "btn-link"

    let makeLarge btn = btn |> addClass "btn-large"
    let makeSmall btn = btn |> addClass "btn-small"
    let makeMini btn = btn |> addClass "btn-mini"
    let makeDisabled btn = btn |> addClass "disabled"
    let makeBlock btn = btn |> addClass "btn-block"

[<JS>] 
module Badge =
    let normal = span |> addClass "badge"

    module Class =
        let success = "badge-success"
        let warning = "badge-warning"
        let important = "badge-important"
        let info = "badge-info"
        let inverse = "badge-inverse"

[<JS>] 
module Label =
    let normal = span |> addClass "label"

    module Class =
        let success = "label-success"
        let warning = "label-warning"
        let important = "label-important"
        let info = "label-info"
        let inverse = "label-inverse"

[<JS>]
module Icon =
    let makeWhite i = i |> addClass "icon-white"

[<JS>]
module Alignment =
    let pullRight = "pull-right"
