namespace Twaddler

open FunScript
open DOM

[<JS>]
type AppState = AppState of Tag[] * (unit -> AppState Async)