<Query Kind="FSharpProgram" />

let DumpAs (name:string) x =
  x.Dump name

let Peek (name:string) x =
  x.Dump name
  x

"1,2,3,4,5,6"
    .Split(',')
    |> Seq.map int
    |> Peek "integers"
    |> Seq.map (fun x -> x * x)
    |> Peek "squares"
    |> Seq.map (sprintf "%d")
    |> String.concat ","
    |> DumpAs "Result"
