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

let addClass (c : string) tagId =
    JS.``$``.Invoke("#" + (tagId())).addClass(c) |> ignore