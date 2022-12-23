namespace rvinowise.ai.figure
    open FsUnit
    open Xunit
    open System.Collections.Generic
    open rvinowise.ai
    open rvinowise.extensions

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edge =
        
        let next_edges 
            (edges: Edge seq)
            (edge: Edge)
            =
            edges
            |>Seq.filter (fun e->
                e.tail.id = edge.head.id
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let all_subfigures edges =
            (edges: Edge seq)
            |>Seq.collect (fun e->[e.tail; e.head])
            |>Seq.distinct
        
        let first_subfigures edges =
            edges
            |>all_subfigures
            |>Seq.filter (
                fun s->
                    edges
                    |> Seq.exists (fun e-> e.head = s)
                    |> not
                )
            |>Seq.distinct


        let subfigures_with_ids ids edges  =
            edges
            |>all_subfigures
            |>Seq.filter (fun subfigure->
                ids
                |>Set.exists (fun id -> 
                    subfigure.id = id
                )
            )

        let starting_from (subfigure: Node_id) (edges: Edge seq) =
            edges
            |>Seq.filter (fun e->e.tail.id = subfigure)
            |>Seq.map (fun e->e.head)


        let rec private all_subfigures_reacheble_from_subfigures
            (reached_goals: HashSet<Node_id>)
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigures: Node_id seq)
            =
            let further_subfigures =
                starting_subfigures
                |>Seq.collect step_further
                |>Subfigures.ids
            
            if Seq.length further_subfigures > 0 then
                further_subfigures
                |>Seq.iter (fun subfigure -> 
                    reached_goals.Add(subfigure) |> ignore
                )

                further_subfigures
                |>all_subfigures_reacheble_from_subfigures 
                    reached_goals 
                    step_further
            else
                ()
        
        let private all_subfigures_reacheble_from_subfigure
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigure: Node_id)
            =
            let reached_goals = HashSet<Node_id>()
            all_subfigures_reacheble_from_subfigures
                reached_goals
                step_further
                [starting_subfigure]
            reached_goals

        let private subfigures_reacheble_from_every_subfigure
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigures: Node_id seq)
            =
            starting_subfigures
            |>Seq.map (all_subfigures_reacheble_from_subfigure step_further)
            |>HashSet.intersectMany


        let subfigures_reacheble_from_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_before_goals: Node_id seq)
            =
            subfigures_before_goals
            |>subfigures_reacheble_from_every_subfigure
                (Subfigure.next_subfigures figure_in_which_search.edges)
    
        let subfigures_reaching_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_after_goals: Node_id seq)
            =
            subfigures_after_goals
            |>Seq.map (
                all_subfigures_reacheble_from_subfigure
                    (Subfigure.previous_subfigures figure_in_which_search.edges)
            )
            |>HashSet.intersectMany


        [<Fact>]
        let ``subfigures reaching others``()=
            subfigures_reaching_other_subfigures
                figure.Example.a_high_level_relatively_simple_figure
                [
                    "b1";"f1"
                ]
            |> should equal ["b0"]
        [<Fact>]
        let ``subfigures reacheble from others``()=
            subfigures_reacheble_from_other_subfigures
                figure.Example.a_high_level_relatively_simple_figure
                [
                    "b0";"b2"
                ]
            |> should equal ["f1"]

namespace rvinowise.ai.stencil
    open rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let all_nodes edges =
            (edges: Edge seq)
            |>Seq.collect (fun e->[e.tail; e.head])
            |>Seq.distinct
        
        let first_nodes edges =
            edges
            |>all_nodes
            |>Seq.filter (
                fun s->
                    edges
                    |> Seq.exists (fun e-> e.head = s)
                    |> not
                )
            |>Seq.distinct

        let starting_from (node: Node_id) (edges: Edge seq) =
            edges
            |>Seq.filter (fun e->e.tail.id = node)
            |>Seq.map (fun e->e.head)

        