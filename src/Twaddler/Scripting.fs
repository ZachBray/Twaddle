[<FunScript.JS>]
module Scripting

open FunScript
open FunScript.TypeScript

type JS = Api< """
    ..\..\data\typescript\lib.d.ts
    ..\..\data\typescript\jquery.d.ts
""" >

let setText (value : string) tagId = 
    JS.``$``.Invoke("#" + (tagId())).text(value) |> ignore

let setHtml (value : string) tagId = 
    JS.``$``.Invoke("#" + (tagId())).html(value) |> ignore

let onClick f tagId =
    JS.``$``.Invoke("#" + (tagId())).click(fun _ -> f(); ()) |> ignore

let appendClass (c : string) tagId =
    JS.``$``.Invoke("#" + (tagId())).addClass(c) |> ignore

[<JSEmit("return {0} == {1};")>]
let areJSEqual(x:'a, y:'a) : bool = failwith "never"

let (===) x y = areJSEqual(x, y)

let tryFindItem (key : string) =
    let item = JS.localStorage.getItem(key)
    if item === null then None
    else Some(item :?> string)

let addItem (key : string) (value : string) =
    JS.localStorage.setItem(key, value) |> ignore

let stringify x = JS.JSON.stringify x
let parse<'a> str = JS.JSON.parse str :?> 'a