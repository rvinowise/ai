namespace rvinowise.ai

open rvinowise.ai.figure
open Expected_figure_prolongation

module Expecting_figures = 


    let get_next_subfigures 
        (edges: Edge seq)
        (subfigure: Subfigure) 
        =
        edges
        |>Seq.filter (fun e->e.tail = subfigure)
        |>Seq.map (fun e->e.head)
        |>Seq.toList

    let prolongate_a_figure_with_an_input_figure 
        fired_figure expectation 
        =
        let expected_figures = 
            expectation.expected
            |>Set.map 
                (fun expected->expected.referenced) 
                
        if (Seq.contains fired_figure expected_figures) then
            let fired_subfigures = 
                Set.filter 
                    (fun (expected: Subfigure) -> expected.referenced = fired_figure)
                    expectation.expected
            let new_expected = 
                fired_subfigures
                |>Seq.collect
                    (get_next_subfigures
                        expectation.prolongated.edges)
                |>Set.ofSeq
            let next_expected = 
                expectation.expected
                |>Set.difference fired_subfigures
                |>Set.union new_expected
            {expectation with expected = next_expected}
        else
            expectation

    let change_expectations_with_new_input 
        (expectations: Expected_figure_prolongation seq) 
        figure_id 
        =
        expectations
        |> Seq.map (prolongate_a_figure_with_an_input_figure figure_id)

