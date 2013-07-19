namespace Twaddler

open FunScript
open DOM
open Html

[<JS>]
type AppState = AppState of Tag[] * (unit -> AppState Async)

[<JS; AutoOpen>]
module EnglishDictionary =
    [<JSEmit("return words;")>]
    let getWords() : string[] = failwith "never"

    [<JSEmit("return definitions;")>]
    let getDefinitions() : string[] = failwith "never"

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

    let getWordScore (str : string) =
        let score = ref 0
        str.ToUpper() |> String.iter (fun c ->
            score := !score + getScore(c.ToString()))
        !score

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
    let title = "app-title"
    let definition = "word-definition"
    let menu = "menu-area"
    let statsTable = "stats-table"

[<JS>]
module Animate =
    let tada tag = tag |> addClass "tada"

[<JS>]
module Statistics =
    open Scripting

    let mutable isLoaded = false
    let mutable gameScores = [||] : int []
    let mutable troublesomeWords = [||] : int []

    let ensureLoaded() =
        if not isLoaded then
            match tryFindItem "gameScores" with
            | Some storedScores ->
                gameScores <- storedScores |> parse<int []>
            | None -> ()
            match tryFindItem "troublesomeWords" with
            | Some storedWords ->
                troublesomeWords <- storedWords |> parse<int []>
            | None -> ()
            isLoaded <- true

    let save() =
        stringify gameScores 
        |> addItem "gameScores"
        stringify troublesomeWords
        |> addItem "troublesomeWords"

    let addTroublesomeWord i =
        ensureLoaded()
        troublesomeWords <- Array.append [|i|] troublesomeWords
        save()

    let removeTroublesomeWord i =
        ensureLoaded()
        troublesomeWords <- troublesomeWords |> Array.filter((<>) i)
        save()

    let getTroublesomeWords() =
        ensureLoaded()
        troublesomeWords

    let calcHighScore() = 
        ensureLoaded()
        if gameScores.Length = 0 then 0
        else gameScores |> Array.max

    let calcAverageScore() = 
        ensureLoaded()
        if gameScores.Length = 0 then 0.0
        else Scripting.JS.Math.floor(gameScores |> Array.averageBy float)

    let addScore score =
        ensureLoaded()
        gameScores <- Array.append [|score|] gameScores
        save()

    let getGamesPlayed() =
        ensureLoaded()
        gameScores.Length


    type Snapshot =
        { GamesPlayed : int
          HighScore : int
          AverageScore : float }

    let takeSnapshot() =    
        { GamesPlayed = getGamesPlayed()
          HighScore = calcHighScore()
          AverageScore = calcAverageScore() } 