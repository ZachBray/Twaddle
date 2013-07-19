[<FunScript.JS>]
module Twaddler.Page.Index

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler

let playId() = "play"
let howItWorksId() = "how-it-works"

let createPage() = [|
    
    Container.row
    |> addKids [|
        Scrabble.tiles "Twaddle"
        |> addClass Style.title
    |]

    Container.row
        |> addKids [|
            Container.offsetSpan 2 8
            |> addKids [|

                Label.normal
                |> addClass Label.Class.warning
                |> addClass Alignment.pullRight
                |> addKids [|
                    span |> addRaw "High Score:"
                    span |> addRaw (Twaddler.Statistics.calcHighScore().ToString())
                    Icons.Star |> Icon.makeWhite
                |]
            |]
        |]

    Container.row
    |> addKids [|
        Container.offsetSpan 2 8
        |> addKids [|
            Button.primary 
            |> addId playId
            |> addKids [|
                Icons.Play |> Icon.makeWhite
                span |> addRaw "Play"
            |]
            |> Button.makeLarge
            |> Button.makeBlock

            Button.info
            |> addId howItWorksId
            |> addKids [|
                Icons.InfoSign |> Icon.makeWhite
                span |> addRaw "How It Works"
            |]
            |> Button.makeLarge
            |> Button.makeBlock

            Button.info
            |> addKids [|
                Icons.Tasks |> Icon.makeWhite
                span |> addRaw "Stats"
            |]
            |> Button.makeLarge
            |> Button.makeBlock
        |]
        |> addClass Style.menu
    |]
|]

let rec next() =
    AppState(
        createPage(),
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                playId |> onClick (fun () -> onNext(Twaddler.Page.Game.next next 3 0))
                howItWorksId |> onClick (fun () -> onNext(Twaddler.Page.HowItWorks.next next))
            ))