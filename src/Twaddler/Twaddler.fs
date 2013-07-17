namespace Twaddler

open FunScript
open DOM
open Html

[<JS>]
type AppState = AppState of Tag[] * (unit -> AppState Async)

[<JS>]
module Scrabble =

    let getScore = function
        | "A" | "E" | "I" | "O" | "U" 
        | "L" | "N" | "R" | "S" | "T" -> 1
        | "D" | "G" -> 2
        | "B" | "C" | "M" | "P" -> 3
        | "F" | "H" | "V" | "W" | "X" | "Y" -> 4
        | "K" -> 5
        | "J" | "X" -> 8
        | "Q" | "Z" -> 10
        | _ -> 0

    let tiles (str : string) =
        let str = str.ToUpper()
        let words = str.Split [|' '|]
        let tiles =
            words |> Array.mapi (fun i str ->
                let tiles = Array.zeroCreate str.Length
                str |> String.iteri (fun j c ->
                    let c = c.ToString()
                    tiles.[j] <- 
                        span |> addClass ("char" + (j + 1).ToString()) 
                        |> addKids [|
                            span |> addRaw c
                            sub |> addRaw ((getScore c).ToString())
                        |])
                span |> addClass ("word" + (i + 1).ToString()) |> addKids tiles
            )
        h 2 |> addClass "cs-text" |> addKids tiles


[<JS>]
module Style =
    let title tag = tag |> addClass "app-title"

[<JS>]
module Animate =
    let tada tag = tag |> addClass "tada"