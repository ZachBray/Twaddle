module Twaddler.Page.Index

open DOM
open Html
open Bootstrap

let create() = [
    Container.offsetSpan 4 8
    |> addKids [
        Button.success 
        |> addAttrs [
            "href" <== "game.html"
        ]
        |> addKids [
            Icons.Play |> Icon.makeWhite
            span |> addRaw "Play"
        ]
        |> Button.makeLarge
        |> Button.makeBlock

        Button.info
        |> addAttrs [
            "href" <== "highscores.html"
        ]
        |> addKids [
            Icons.InfoSign |> Icon.makeWhite
            span |> addRaw "High Scores"
        ]
        |> Button.makeLarge
        |> Button.makeBlock

        Button.danger
        |> addKids [
            Icons.Eject |> Icon.makeWhite
            span |> addRaw "Exit"
        ]
        |> Button.makeLarge
        |> Button.makeBlock
    ]
]

