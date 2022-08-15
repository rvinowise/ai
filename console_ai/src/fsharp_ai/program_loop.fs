module rvinowise.ai.ui.console

open System

open rvinowise.ai

let print_prompt () =
    printf "\n:>"

let print_error () =
    printfn "unknown command"

let read_input _ =
    print_prompt()
    System.Console.ReadLine()

let appearances_of_figure name =
    match loaded.Figure.with_id name with
        | Some found_figure -> printed.Figure.appearances found_figure
        | None -> printf "no such figure."

let subfigures_of_figure name =
    match loaded.Figure.with_id name with
        | Some found_figure -> painted.Figure.internal_graph found_figure
        | None -> printf "no such figure."

let process_input (command:string) =
    match command.Split [|' '|] with
    | [|"show"; args|] -> appearances_of_figure args
    | [|"quit"|] | [|"exit"|] -> Environment.Exit 55
    | _ -> print_error()

let program_loop =
    read_input
    |> Seq.initInfinite
    |> Seq.iter process_input