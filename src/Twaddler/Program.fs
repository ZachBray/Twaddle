open DOM
open Html
open Bootstrap

let buttons = [
    Button.success 
    |> addKids [
        Icons.Play |> Icon.makeWhite
        span |> addRaw "Play"
    ]
    |> Button.makeLarge
    |> Button.makeBlock

    Button.info
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

let twaddlerIndex =
    Bootstrap.Document.create "index.html" "Twaddler" [] buttons

let root = __SOURCE_DIRECTORY__ + @"\..\..\www"
Compiler.compile root twaddlerIndex