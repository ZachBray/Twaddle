[<FunScript.JS>]
module Twaddler.Page.Game

open DOM
open Html
open Bootstrap
open Scripting
open Twaddler
open FunScript
open FunScript.TypeScript

let definitionId() = "definition-block"
let wordId i () = "word-" + i.ToString()

let createPage def options livesLeft score =
    let words = getWords()
    [|
        Container.row
        |> addKids [|
            div |> addKids [|
                Scrabble.tiles "Pick The"
                Scrabble.tiles "Word"
            |]
            |> addClass Style.title
        |]

        Container.row
        |> addKids [|
            Container.offsetSpan 2 8
            |> addKids [|
                Label.normal 
                |> addClass Label.Class.important
                |> addKids [|
                    yield span |> addRaw "Lives:"
                    for i = 1 to livesLeft do
                        yield Icons.Heart |> Icon.makeWhite 
                |]

                Label.normal
                |> addClass Label.Class.warning
                |> addClass Alignment.pullRight
                |> addKids [|
                    span |> addRaw "Score:"
                    span |> addRaw (score.ToString())
                    Icons.Star |> Icon.makeWhite
                |]
            |]
        |]

        Container.row |> addKids [|
            Container.offsetSpan 2 8 |> addKids [|
                Button.inverse 
                |> addRaw def
                |> Button.makeDisabled 
                |> Button.makeLarge 
                |> Button.makeBlock
                |> addClass Style.definition
            |]
        |]

        Container.row |> addKids [|
            Container.offsetSpan 2 8  |> addKids (
                options |> Array.map (fun i ->
                    Button.normal |> addRaw words.[i] |> addId (wordId i)
                    |> addAttrs [| "href" <== "#" |] |> Button.makeLarge |> Button.makeBlock
                ))
        |]
    |]



let randomInt (exclusiveUpper : int) =
    JS.Math.floor(JS.Math.random() * float exclusiveUpper)
    |> int

let withRatio fNum gNum f g =
    let threshold = fNum
    let i = randomInt(fNum + gNum)
    if i < fNum then f()
    else g()

let rec next goHome livesLeft score =
    let words = getWords()
    let definitions = getDefinitions()
    let i, isTroublesome = 
        withRatio 70 30
            (fun () -> randomInt words.Length, false)
            (fun () -> 
                let troublesomeWords = Statistics.getTroublesomeWords()
                let l = troublesomeWords.Length
                if l > 0 then troublesomeWords.[randomInt l], true
                else randomInt words.Length, false)
    let realIndex = i
    let rec pickNext picked k =
        let j = (randomInt(10 + k) - 5 + i + words.Length) % words.Length 
        let word = words.[j]
        let usedAlready = picked |> Array.exists ((=) word)
        if usedAlready then pickNext picked (k+1)
        else j
    let j = pickNext [|words.[i]|] 0
    let k = pickNext [|words.[i]; words.[j]|] 0
    let options = [|i; j; k|] |> Array.sortBy (fun _ -> randomInt 10)
    AppState(
        createPage definitions.[i] options livesLeft score,
        fun () ->
            Async.FromContinuations(fun (onNext, _, _) ->
                options |> Array.iter (fun i ->
                    wordId i |> onClick (fun () -> 
                        async {
                            let livesLeft, points =
                                if i = realIndex then
                                    wordId i |> appendClass "btn-success"
                                    wordId i |> appendClass "animated"
                                    wordId i |> appendClass "tada"
                                    let word = words.[i]
                                    let points = Scrabble.getWordScore word
                                    if isTroublesome then
                                        withRatio 80 20 ignore (fun () -> 
                                            Statistics.removeTroublesomeWord i)
                                    livesLeft, points
                                else
                                    wordId realIndex |> appendClass "btn-info"
                                    wordId i |> appendClass "btn-danger"
                                    wordId i |> appendClass "animated"
                                    wordId i |> appendClass "wobble"
                                    Statistics.addTroublesomeWord i
                                    livesLeft - 1, 0
                            do! Async.Sleep 800
                            if livesLeft = 0 then
                                let oldSnap = Statistics.takeSnapshot()
                                Statistics.addScore score
                                let newSnap = Statistics.takeSnapshot()
                                onNext(Stats.next(oldSnap, newSnap, goHome))
                            else
                                onNext(next goHome livesLeft (score + points))
                        } |> Async.StartImmediate))
            ))