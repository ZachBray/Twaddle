[<FunScript.JS>]
module Twaddler.Page.HowItWorks

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler

let backId() = "back"

let descriptionText() = 
    [| 
        p |> addRaw """Twaddle is a simple game.
We present you with a definition.
You then try to pick the right word."""
    
        p |> addRaw """If you succeed, you win points. 
If you fail, you lose a life."""

        p |> addRaw """
Points are calculated using scrabble tile scores."""
    
        p |> addRaw """
You start with three lives. 
When you lose your last life,
the score you have accumulated up to that point is your final score."""

    |]


let createPage() = [|
    
    Container.row
    |> addKids [|
        div |> addKids [|
                Scrabble.tiles "How It"
                Scrabble.tiles "Works"
            |]
            |> addClass Style.title
    |]

    Container.row |> addKids [|
        Container.offsetSpan 2 8 |> addKids [|
            Button.inverse 
            |> addKids (descriptionText())
            |> Button.makeDisabled 
            |> Button.makeLarge 
            |> Button.makeBlock
            |> addClass Style.definition
        |]
    |]

    Container.row
    |> addKids [|
        Container.offsetSpan 2 8
        |> addKids [|
            Button.primary 
            |> addId backId
            |> addKids [|
                Icons.ChevronLeft |> Icon.makeWhite
                span |> addRaw "Back"
            |]
            |> Button.makeLarge
            |> Button.makeBlock
        |]
    |]
|]

let rec next(callback) =
    AppState(
        createPage(),
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                backId |> onClick (fun () -> onNext(callback()))
            ))