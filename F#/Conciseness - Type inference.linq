<Query Kind="FSharpProgram" />

let Where source predicate =
  Seq.filter predicate source

let GroupBy source keySelector =
  Seq.groupBy keySelector source

let i = 1
let s = "hello"
let tuple = s, i
let s2, i2 = tuple
let list = [s2]

// Create an impromptu tuple so I can dump the results
(i, s, tuple, s2, i2, list) |> Dump |> ignore

let sumLengths strList =
  strList |> List.map String.length |> List.sum

["Hello"; "World"; "How are you today?"; "I'm fine, thanks for asking"] |> Dump |> sumLengths |> Dump |> ignore