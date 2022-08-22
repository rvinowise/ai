module rvinowise.ai.ui.console

open System

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai

let print_prompt () =
    printf "\n:>"

let print_error message =
    printfn message

let read_input _ =
    print_prompt()
    System.Console.ReadLine()

    
type Ai_entity =
    | Figure
    | Appearance
    | Edge
    | None

let entity_mentioned_in_command part =
    match part with
    | "appearances"|"edges" -> Ai_entity.Figure
    | _ -> None

//let show_parts_of_figure part figure_id =
//    try
//        match part with
//        | "appearances" -> 
//            ui.printed.Figure.appearances 
//                (loaded.figure.Appearances.of_figure figure_id)
//        | "edges" ->
//            ui.painted.Figure.edges 
//                figure_id
//                (loaded.figure.Edges.of_figure figure_id)
//        | _ -> print_error "unknown entity to show"
//    with
//        | :? Xml.XmlException
//        | :? Configuration.ConfigurationErrorsException
//        | :? TypeInitializationException as e->
//            printf $"loading from the database failed, bad configuration file:\n%s{e.Message}"
//        | e->
//            printfn $"failed to show the entity %s{part}:\n %s{e.Message}"

let show_something_about_figure figure shown_part =
    if loaded.Figure.exists figure then
        match shown_part with
        | "appearances" ->
            ui.printed.Figure.appearances figure
                (ai.loaded.figure.Appearances.all_appearances figure)
        | "edges" ->
            ui.painted.Figure.edges figure
                (loaded.figure.Edges.edges figure)
        | _ -> print_error $"%s{shown_part} is not part of a figure"
    else
        print_error $"figure %s{figure} doesn't exist"
       

let show_entity entity_type exemplar_id =
    match entity_mentioned_in_command entity_type with
        | Figure ->
            show_something_about_figure
                exemplar_id entity_type
        | _ -> print_error $"entity %s{entity_type} doesn't exist" 

let add_signal signal_id =
    database.Write.new_signal signal_id

let add_entity entity_type exemplar_id =
    match entity_type with
        | "signal" ->
            add_signal exemplar_id
        | _ -> print_error $"entity %s{entity_type} doesn't exist"
    
    
let input_sensory_data (data: string) =
    //let figures =  data.Split [|' '|]
    data
    |> Seq.iter (fun figure_id ->
        let figure_id = string figure_id
        if loaded.Figure.exists <| figure_id then
            created.figure.Appearance.new_input figure_id
        else
            print_error $"figure ${figure_id} doesn't exist"
    )

let find_sequences _ =
    Finding_sequences.find_repeated_pairs("teststr2", 6, 6.66)

let process_input (command:string) =
    match command.Split [|' '|] |> Array.toList with
    | ["show";entity;name] -> show_entity entity name
    | ["add";entity;name] -> add_entity entity name
    | ["input";sensory_data] -> input_sensory_data sensory_data
    | ["find"] -> find_sequences()
    | ("quit" | "exit") :: _ -> Environment.Exit 55
    | _ -> print_error "unknown command"



let program_loop =
    read_input
    |> Seq.initInfinite
    |> Seq.iter process_input