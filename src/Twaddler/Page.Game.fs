[<FunScript.JS>]
module Twaddler.Page.Game

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler
open FunScript
open FunScript.TypeScript

let definitionId() = "def-id"
let word1Id() = "word1-id"
let word2Id() = "word2-id"
let word3Id() = "word3-id"

let createPage def w1 w2 w3 = [|
    Container.row
    |> addKids [|
        Scrabble.tiles "Pick the word"
        |> Style.title
    |]

    Container.row |> addKids [|
        Container.offsetSpan 2 8 |> addKids [|
            Container.hero |> addKids [|
                p |> addRaw def
                p |> addKids [|
                    Button.primary |> addRaw w1 |> addId word1Id
                    |> addAttrs [| "href" <== "#" |] |> Button.makeLarge //|> Button.makeBlock
                    Button.primary |> addRaw w2 |> addId word2Id
                    |> addAttrs [| "href" <== "#" |] |> Button.makeLarge //|> Button.makeBlock
                    Button.primary |> addRaw w3 |> addId word3Id
                    |> addAttrs [| "href" <== "#" |] |> Button.makeLarge //|> Button.makeBlock
                |]
            |]
        |]
    |]
|]

[<JSEmit("return words;")>]
let getWords() : string[] = failwith "never"

[<JSEmit("return definitions;")>]
let getDefinitions() : string[] = failwith "never"

let randomInt (exclusiveUpper : int) =
    JS.Math.floor(JS.Math.random() * float exclusiveUpper)
    |> int

let rec next() =
    let words = getWords()
    let definitions = getDefinitions()
    let i = randomInt words.Length
    let j = (randomInt 10 + i) % words.Length
    let k = if i = j then j + 1 else j
    let k = (randomInt 10 + i) % words.Length
    let k = if j = k then k + 1 else k
    AppState(
        createPage definitions.[i] words.[i] words.[j] words.[k],
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                word1Id |> onClick (fun () -> onNext(next()))
                word2Id |> onClick (fun () -> onNext(next()))
                word3Id |> onClick (fun () -> onNext(next()))
            ))