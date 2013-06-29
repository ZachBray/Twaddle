[<FunScript.JS>]
module Scripting

open FunScript
open FunScript.TypeScript

type JS = Api< """
    ..\..\data\typescript\lib.d.ts
    ..\..\data\typescript\jquery.d.ts
""" >

let setText (value : string) tagId = 
    JS.``$``.Invoke("#" + tagId).text(value) |> ignore