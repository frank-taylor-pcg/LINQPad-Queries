<Query Kind="FSharpProgram" />

// This was surprisingly easy to figure out
let dmp x = x |> Dump |> ignore

let myInt = 5
let myFloat = 3.14
let myString = "hello"

let twoToFive = [2;3;4;5]
let oneToFive = 1 :: twoToFive
let zeroToFive = [0;1] @ twoToFive

let square x = x * x
square 3 |> dmp

let add x y = x + y
add 2 3 |> dmp

let evens list =
  let isEven x = x % 2 = 0
  List.filter isEven list

evens oneToFive |> dmp

let sumOfSquaresTo100 = List.sum( List.map square [1..100] ) |> dmp
let sumOfSquaresTo100Piped = [1..100] |> List.map square |> List.sum |> dmp
let sumOfSquaresTo100WithFunc = [1..100] |> List.map (fun x -> x * x) |> List.sum |> dmp

let simplePatternMatch =
  let x = "a"
  match x with
  | "a" -> printfn "x is a"
  | "b" -> printfn "x is b"
  | _ -> printfn "x is something else" // underscore matches anything (like with Elixir)

// Some(..) and None are roughly analogous to Nullable wrappers
let validValue = Some(99)
let invalidValue = None

let optionPatternMatch input =
  match input with
  | Some i -> printfn "input is an int = %d" i
  | None -> printfn "input is missing"

optionPatternMatch validValue |> dmp
optionPatternMatch invalidValue |> dmp

let twoTuple = 1, 2
let threeTuple = "a", 2, true

type Person = { First:string; Last:string }
let frank = { First = "Frank"; Last = "Taylor" }
frank |> dmp

type Temp =
  | DegreesC of float
  | DegreesF of float
let temp = DegreesF 98.6
temp |> dmp

type Employee =
  | Worker of Person
  | Manager of Employee list

let worker = Worker frank
worker |> dmp

printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
printfn "A string %s, and something generic %A" "hello" [1;2;3;4]

printfn "twoTuple = %A,\nPerson = %A,\nTemp = %A,\nEmployee = %A"
  twoTuple frank temp worker