[<FunScript.JS>]
module Twaddler.Page.Game

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler
open FunScript
open FunScript.TypeScript

let definitionId() = "definition-block"
let wordId i () = "word-" + i.ToString()

[<JSEmit("return words;")>]
let getWords() : string[] = failwith "never"

[<JSEmit("return definitions;")>]
let getDefinitions() : string[] = failwith "never"

let createPage def options =
    let words = getWords()
    [|
        Container.row
        |> addKids [|
            Scrabble.tiles "Pick the word"
            |> Style.title
        |]

        Container.row |> addKids [|
            Container.span 12 |> addKids [|
                Button.inverse 
                |> addRaw def
                |> Button.makeDisabled 
                |> Button.makeLarge 
                |> Button.makeBlock
                |> Style.definition
            |]
        |]

        Container.row |> addKids [|
            Container.span 12 |> addKids (
                options |> Array.map (fun i ->
                    Button.normal |> addRaw words.[i] |> addId (wordId i)
                    |> addAttrs [| "href" <== "#" |] |> Button.makeLarge |> Button.makeBlock
                ))
        |]
    |]



let randomInt (exclusiveUpper : int) =
    JS.Math.floor(JS.Math.random() * float exclusiveUpper)
    |> int

let rec next() =
    let words = getWords()
    let definitions = getDefinitions()
    let i = randomInt words.Length
    let realIndex = i
    let rec pickNext picked k =
        let j = (randomInt(10 + k) - 5 + i + words.Length) % words.Length 
        let word = words.[j]
        let usedAlready = picked |> Array.exists ((=) word)
        if usedAlready then pickNext picked (k+1)
        else j
    let j = pickNext [|words.[i]|] 0
    let k = pickNext [|words.[i]; words.[j]|] 0
    let options = [|i; j; k|] |> Array.sortBy (fun _ -> randomInt 10)
    AppState(
        createPage definitions.[i] options,
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                options |> Array.iter (fun i ->
                    wordId i |> onClick (fun () -> 
                        async {
                            if i = realIndex then
                                wordId i |> addClass "btn-success"
                                wordId i |> addClass "animated"
                                wordId i |> addClass "tada"
                            else
                                wordId realIndex |> addClass "btn-info"
                                wordId i |> addClass "btn-danger"
                                wordId i |> addClass "animated"
                                wordId i |> addClass "wobble"
                            do! Async.Sleep 800
                            onNext(next())
                        } |> Async.StartImmediate))
            ))