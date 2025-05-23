namespace rvinowise.ai
open rvinowise.ai


module Expecting_figures = 

    let prolongate_expectation_with_an_input_figure 
        (fired_figure: Figure_id)
        (expectation: Expected_figure_prolongation) 
        =
        let expected_figures = 
            expectation.expected
            |>Figure.referenced_figures expectation.prolongated
            |>Seq.map _.name
        if (Seq.contains fired_figure expected_figures) then
            let fired_subfigures = 
                expectation.expected
                |>Set.filter (fun expected -> 
                    expected
                    |>Figure.reference_of_vertex expectation.prolongated
                    |>_.name
                        = fired_figure
                )
                    
            let new_expected = 
                fired_subfigures
                |>Seq.collect(
                    Edges.next_vertices
                        expectation.prolongated.edges) 
                |>Set.ofSeq
                

            let updated_expected = 
                fired_subfigures
                |>Set.difference expectation.expected
                |>Set.union new_expected
            //let updated_expected = expectation.expected - fired_subfigures + new_expected
            {expectation with expected = updated_expected}
        else
            expectation

    let change_expectations_with_new_input 
        (expectations: Expected_figure_prolongation seq) 
        figure_id
        =
        expectations
        |> Seq.map (
            prolongate_expectation_with_an_input_figure figure_id
        )

