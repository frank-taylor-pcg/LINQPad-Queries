<Query Kind="FSharpProgram" />

// This type must have both components
type IntAndBool = { intPart : int; boolPart: bool }

let x = { intPart = 1; boolPart = false }

// This type has one or the other component type (but not both?)
type IntOrBool =
  | IntChoice of int
  | BoolChoice of bool

let y = IntChoice 42
let z = BoolChoice true

y |> Dump |> ignore
z |> Dump |> ignore

type Shape = // define a "union" of alternative structures
  | Circle of radius:int
  | Rectangle of height:int * width:int
  | Point of x:int * y:int
  | Polygon of pointList:(int * int) list

let draw shape = // define a function "draw" with a shape parameter
  match shape with
  | Circle radius ->
    printfn "The circle has a radius of %d" radius
  | Rectangle (height, width) ->
    printfn "The rectangle is %d high by %d wide" height width
  | Polygon points ->
    printfn "The polygon is made of these points %A" points
  | _ ->
    printfn "I don't recognize this shape"

let circle = Circle(10)
let rect = Rectangle(4, 5)
let point = Point(2, 3)
let polygon = Polygon( [(1, 1); (2, 2); (3, 3)] )

[circle; rect; point; polygon] |> List.iter draw