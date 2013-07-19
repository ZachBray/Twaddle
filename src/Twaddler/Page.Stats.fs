[<FunScript.JS>]
module Twaddler.Page.Stats

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler

let backId() = "back"

let createStat o n f =
    let o = f o
    let n = f n
    let delta = n - o
    n.ToString(),
    (if delta > 0 then 
        Some(Icons.ArrowUp, delta.ToString())
     elif delta = 0 then None
     else Some(Icons.ArrowDown, delta.ToString()))

let statsTable (o : Statistics.Snapshot) (n : Statistics.Snapshot) =
    let info = [|
        "Games Played", Label.Class.success, createStat o n (fun x -> x.GamesPlayed)
        "High Score", Label.Class.warning, createStat o n (fun x -> x.HighScore)
        "Average/Mean Score", Label.Class.warning, createStat o n (fun x -> int x.AverageScore)
        "Total Words", Label.Class.info, (getWords().Length.ToString(), None)
    |]

    Container.table
    |> addKids [|
        for title, labelClass, (desc, delta) in info do
            yield tableRow |> addKids [|
                tableCell |> addRaw title
                tableCell |> addKids [| 
                    Label.normal |> addClass labelClass
                    |> addRaw desc
                |]
                tableCell |> addKids [|
                    match delta with
                    | None -> ()
                    | Some(icon, delta) ->
                        yield
                            Label.normal
                            |> addClass labelClass
                            |> addKids [|
                                span |> addRaw delta
                                icon |> Icon.makeWhite
                            |]

                |]
            |]
    |]
    |> addClass Table.Classes.bordered
    |> addClass Style.statsTable


let createPage oldSnapshot newSnapshot = [|
    
    Container.row
    |> addKids [|
        div |> addKids [|
                Scrabble.tiles "Stats"
            |]
            |> addClass Style.title
    |]

    Container.row |> addKids [|
        Container.offsetSpan 2 8 |> addKids [|
             statsTable oldSnapshot newSnapshot
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

let rec next(oldSnapshot, newSnapshot, callback) =
    AppState(
        createPage oldSnapshot newSnapshot,
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                backId |> onClick (fun () -> onNext(callback()))
            ))