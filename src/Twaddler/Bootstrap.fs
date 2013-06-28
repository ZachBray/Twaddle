namespace Bootstrap

open DOM
open Html

module Document =
    let create path name head content =
        {
            Path = path
            Header = 
                [
                    yield title name
                    yield meta |> addAttrs [
                        "name" <== "viewport"
                        "content" <== "width=device-width, initial-scale=1.0"
                    ]
                    yield link |> addAttrs [
                        "href" <== "css/bootstrap.min.css" 
                        "rel" <== "stylesheet" 
                        "media" <== "screen"
                    ]
                    yield! head
                ]
            Body =
                [
                    yield script |> addAttrs [
                        "src" <== "js/jquery.min.js"
                    ]
                    yield script |> addAttrs [
                        "src" <== "js/bootstrap.min.js"
                    ]
                    yield! content
                ]
        }

module Button =
    let normal = button |> addClass "btn"
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


module Icon =
    let makeWhite i = i |> addClass "icon-white"
