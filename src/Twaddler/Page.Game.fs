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

let createPage() = [|
    Container.row |> addKids [|
        Container.offsetSpan 2 8 |> addKids [|
            h 3 |> addRaw "Pick-a-word"
            p 
            |> addId definitionId
            |> addRaw "Definition of some word goes here"
            p |> addKids [|
                Button.primary |> addRaw "Word1" |> addId word1Id
                |> addAttrs [| "href" <== "#" |] |> Button.makeBlock
                Button.primary |> addRaw "Word2" |> addId word2Id
                |> addAttrs [| "href" <== "#" |] |> Button.makeBlock
                Button.primary |> addRaw "Word3" |> addId word3Id
                |> addAttrs [| "href" <== "#" |] |> Button.makeBlock
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
        createPage(),
        fun () ->
            definitionId |> setText definitions.[i]
            word1Id |> setText words.[i]
            word2Id |> setText words.[j]
            word3Id |> setText words.[k]
            Async.FromContinuations(fun (onNext, _, _) ->
                word1Id |> onClick (fun () -> onNext(next()))
                word2Id |> onClick (fun () -> onNext(next()))
                word3Id |> onClick (fun () -> onNext(next()))
            ))