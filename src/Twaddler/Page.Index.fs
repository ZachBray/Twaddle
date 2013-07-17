[<FunScript.JS>]
module Twaddler.Page.Index

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler

let playId() = "play-id"

let createPage() = [|
    Container.offsetSpan 4 8
    |> addKids [|
        Button.success 
        |> addId playId
        |> addKids [|
            Icons.Play |> Icon.makeWhite
            span |> addRaw "Play"
        |]
        |> Button.makeLarge
        |> Button.makeBlock

        Button.info
        |> addKids [|
            Icons.InfoSign |> Icon.makeWhite
            span |> addRaw "High Scores"
        |]
        |> Button.makeLarge
        |> Button.makeBlock

        Button.danger
        |> addKids [|
            Icons.Eject |> Icon.makeWhite
            span |> addRaw "Exit"
        |]
        |> Button.makeLarge
        |> Button.makeBlock
    |]
|]

let next() =
    AppState(
        createPage(),
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                playId |> onClick (fun () -> onNext(Twaddler.Page.Game.next()))
            ))