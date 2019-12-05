<Query Kind="FSharpProgram" />

let square x = x * x

let sumOfSquares n =
  [1..n]
  |> List.map square
  |> List.sum

sumOfSquares 100 |> Dump |> ignore

