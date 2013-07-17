open DOM
open Html
open Scripting
open Twaddler
open FunScript
open System.IO
open System.Diagnostics

let root = __SOURCE_DIRECTORY__ + @"\..\..\www"

let deployDictionary() =
    let dictionary = __SOURCE_DIRECTORY__ + @"\..\..\data\dictionaries\english.js"
    let destination = Path.Combine(root, "js\dictionary.js")
    if File.Exists destination then
        File.Delete destination
    File.Copy(dictionary, destination)

[<JS>]
module Application =
    
    let rootId() = "root-id"

    let rec renderLoop(AppState(page, next)) : Async<unit> =
        async {
            let allTags = page |> Seq.collect Compiler.compileTag
            let html = allTags |> String.concat ""
            rootId |> setHtml html
            let! nextState = next()
            return! renderLoop nextState
        }

    let render state =
        renderLoop state 
        |> Async.StartImmediate


    let main() =
        //Page.Game.createPage() |> ignore
        render(Page.Index.next())

let compileJavaScript() = 
    let body = Compiler.Compiler.Compile(<@ Application.main() @>, components = Interop.Components.all, noReturn = true)
    sprintf "$(document).ready(function () {\n%s\n});" body

let compilePage() =
    let appPage = 
        Bootstrap.Document.create "Twaddler" 
            [|
                script |> addAttrs [|
                    "src" <== "js/dictionary.js"
                |]
            |] 
            [| 
                div |> addId Application.rootId
                script |> addAttrs [|
                    "language" <== "JavaScript"
                |]
                |> addRaw (compileJavaScript())  
            |]

    let lines = DOM.Compiler.compileDoc appPage
    let appPagePath = Path.Combine(root, "index.html")
    File.WriteAllLines(appPagePath, lines |> Seq.toArray)
    Process.Start appPagePath |> ignore

deployDictionary()
compilePage()

//let expr =  
//    <@ fun (ue : FunScript.Core.Seq.UnfoldEnumerator<obj,obj>) -> 
//            let x = ue :> System.Collections.IEnumerator
//            x.MoveNext() @>
//
//open System.Reflection
//let mb = typeof<FunScript.Core.Seq.UnfoldEnumerator<obj,obj>>.GetInterfaceMap(typeof<System.Collections.IEnumerator>).TargetMethods |> Seq.find (fun m -> m.Name.EndsWith "MoveNext")
//
//
//printfn "%A\n\n" (Microsoft.FSharp.Quotations.Expr.TryGetReflectedDefinition mb)
//
//let test = Compiler.compile expr
//        
//printfn "%s" test
//System.Console.ReadLine() |> ignore