<Query Kind="FSharpProgram" />

(*
If the list is empty, there is nothing to do.
Otherwise:
  1. Take the first element of the list
  2. Find all elements in the rest of the list that are less than the first element and sort them
  3. Find all elements in the rest of the list that are >= than the first element and sort them
  4. Combine the three parts together to get the final result
    (sorted smaller elements + firstElement + sorted larger elements)
*)

let rec quicksort list =
  match list with
  | [] ->
    []
  | firstElem::otherElements ->
    let smallerElements =
      otherElements
      |> List.filter (fun e -> e < firstElem)
      |> quicksort
    let largerElements =
      otherElements
      |> List.filter (fun e -> e >= firstElem)
      |> quicksort
    
    List.concat [smallerElements; [firstElem]; largerElements]
 
printfn "%A" (quicksort [1;5;23;18;9;1;3])