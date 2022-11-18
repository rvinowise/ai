namespace rvinowise.ai

open rvinowise.ai.figure

module Spotting_figures = 


    type Expectation_of_figure_prolongation = {
        prolongated: Figure
        last_activated: Subfigure list
        expected: Subfigure list
    }

    let prolongate_a_figure_with_lower_figure expectation lower_figure=
        let expected_figures = List.map (fun expected->expected.referenced) expectation.expected
        if (List.contains lower_figure expected_figures) then
            let next_expected = 
                expectation.expected
                |>
            {expectation with expected = next_expected}
        else
            expectation

    let input_figure 
        (expectations: Expectation_of_figure_prolongation list) 
        figure_id 
        =
        expectations
        |> List.

