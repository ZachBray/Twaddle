module Twaddler.Page.Game

open DOM
open Html
open Bootstrap
open Scripting
open FunScript
open FunScript.TypeScript

[<JS>]
module Script =
    [<JSEmit("return words;")>]
    let getWords() : string[] = failwith "never"

    [<JSEmit("return definitions;")>]
    let getDefinitions() : string[] = failwith "never"

    let randomInt (exclusiveUpper : int) =
        JS.Math.floor(JS.Math.random() * float exclusiveUpper)
        |> int

    let definitionId = "def-id"
    let word1Id = "word1-id"
    let word2Id = "word2-id"
    let word3Id = "word3-id"

    let main() =
        let words = getWords()
        let definitions = getDefinitions()
        let i = randomInt words.Length
        definitionId |> setText definitions.[i]
        word1Id |> setText words.[i]
        let j = (randomInt 10 + i) % words.Length
        let k = if i = j then j + 1 else j
        word2Id |> setText words.[j]
        let k = (randomInt 10 + i) % words.Length
        let k = if j = k then k + 1 else k
        word3Id |> setText words.[k]
        


    let src() = 
        let body = Compiler.Compiler.Compile(<@ main() @>, components = Interop.Components.all, noReturn = true)
        sprintf "$(document).ready(function () {\n%s\n});" body

let create() = [
    Container.row |> addKids [
        Container.offsetSpan 2 8 |> addKids [
            Container.hero
            |> addKids [
                h 1 |> addRaw "Pick-a-word"
                p 
                |> addId Script.definitionId
                |> addRaw "Definition of some word goes here"
                p |> addKids [
                    Button.primary |> addRaw "Word1" |> addId Script.word1Id
                    |> addAttrs [
                        "href" <== "game.html"
                    ]
                    |> Button.makeBlock
                    Button.primary |> addRaw "Word2" |> addId Script.word2Id
                    |> addAttrs [
                        "href" <== "game.html"
                    ]
                    |> Button.makeBlock
                    Button.primary |> addRaw "Word3" |> addId Script.word3Id
                    |> addAttrs [
                        "href" <== "game.html"
                    ]
                    |> Button.makeBlock
                ]
            ]
        ]
    ]

    script |> addAttrs [
        "src" <== "js/dictionary.js"
    ]

    script 
    |> addAttrs [
        "language" <== "JavaScript"
    ]
    |> addRaw (Script.src())
]

