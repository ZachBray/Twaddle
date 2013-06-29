open DOM
open Twaddler
open System.IO
open System.Diagnostics

let twaddlerIndex = Bootstrap.Document.create "index.html" "Twaddler" [] (Page.Index.create())
let pages = [
    twaddlerIndex
    Bootstrap.Document.create "game.html" "Twaddler" [] (Page.Game.create())
]
let dictionary = __SOURCE_DIRECTORY__ + @"\..\..\data\dictionaries\english.js"
let root = __SOURCE_DIRECTORY__ + @"\..\..\www"
let destination = Path.Combine(root, "js\dictionary.js")
if File.Exists destination then
    File.Delete destination
File.Copy(dictionary, destination)
for page in pages do 
    Compiler.compile root page
let index = Path.Combine(root, twaddlerIndex.Path)
Process.Start index |> ignore