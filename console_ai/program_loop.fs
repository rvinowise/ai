module rvinowise.ai.ui.console

open System

open rvinowise.ai
open rvinowise.ai

let print_prompt () =
    printf "\n:>"

let print_error () =
    printfn "unknown command"

let read_input _ =
    print_prompt()
    System.Console.ReadLine()

let show_figure_dialog name =
    match Figure_storage.figure_storage |> Figure_storage.get_figure name with
        | Some found_figure -> Figure.print_appearances_of found_figure
        | None -> printf "no such figure."

let process_input (command:string) =
    match command.Split [|' '|] with
    | [|"show"; args|] -> show_figure_dialog args
    | [|"quit"|] | [|"exit"|] -> Environment.Exit 55
    | _ -> print_error()

let program_loop =
    read_input
    |> Seq.initInfinite
    |> Seq.iter process_input