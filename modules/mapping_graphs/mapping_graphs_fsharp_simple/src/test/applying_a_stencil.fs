namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise.ai.Applying_stencil
open rvinowise.ai.Mapping_graph
open rvinowise.ai.ui
open rvinowise.ai.stencil
open rvinowise.ui
        
        
module ``application of stencils``=
    
    
        
    let initial_mapping_without_prolongation = Mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"]
    let initial_fruitful_mapping = Mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"]
    let initial_useless_mapping = Mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        
    let result_of_fruitful_stencil_application =
        built.Figure.simple
            [
                "d","e"
            ]
              

    [<Fact>]
    let ``impossible to prolongate, because of no matching following subfigures``()=
        prolongate_one_mapping_with_next_subfigures 
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
            [(Vertex_id "f#1", Figure_id "f")]
            initial_mapping_without_prolongation
        |> should equal
            []
    
    [<Fact>]
    let``prolongation with a single following node``()=
        prolongate_one_mapping_with_next_subfigures 
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
            [(Vertex_id "f#1", Figure_id "f")]
            initial_useless_mapping
        |> should equal
            [
                Mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2"
                ]
            ]
    

    
    [<Fact>]
    let ``several mappings can be produced by one stencil application``()=
        
        (
             map_figure_onto_target
                example.Figure.a_high_level_relatively_simple_figure
                example.Figure.fitting_stencil_as_figure
                |> Set.ofSeq
        )
        |>should equal (
            Set.ofSeq [
                Mapping.ofStringPairs [
                    "b#1","b#1";
                    "h#1","h#1";
                    "f#1","f#2"
                ];
                Mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2"
                ]
            ])

    [<Fact>]
    let ``a full mapping can be produced if the stencil has only "out" in the middle``()=
        let mappee = 
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]|>Figure_from_stencil.convert

        let target =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text
   
        let mappings =
            map_figure_onto_target
                mappee
                target
                |>Set.ofSeq
        
        mappings
        |>Seq.iter (fun mapping->
            mapping
            |>Seq.map (fun pair->pair.Key)
            |>Set.ofSeq
            |>should equal (
                ["N";",#1";",#2";";"]
                |>Seq.map Vertex_id
                |>Set.ofSeq
            )
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with several vertices)``()=
        let target = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil
        stencil
        |>results_of_stencil_application target
        |>Set.ofSeq
        |>should equal (
            [
                ["d","e"]|>built.Figure.simple;
                "h"|>built.Figure.signal 
            ]|>Set.ofList
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with only one vertex)``()=
        built.Stencil.simple_with_separator [
            "N","out";
            "out",";";
        ]
        |>results_of_stencil_application (built.Figure.sequential_figure_from_text "N0;")
        |>should equal [
            built.Figure.signal "0"
        ]
    
    [<Fact>]
    let ``mapping of first stencil subfigures onto target produces initial mapping``()=
        map_first_nodes
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
        |>Set.ofSeq
        |> should equal ([   
            Mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"];
            Mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"];
            Mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        ]|>Set.ofSeq)
    
    [<Fact>]
    let ``initial mapping when the target lacks some figures``()=
        map_first_nodes
            (
                built.Figure.simple [
                    "b","f";
                    "h","f";
                    "x","f";
                ]
            )
            example.Figure.a_high_level_relatively_simple_figure
        |> should be Empty

    

    
    
    [<Fact>]
    let ``complete mapping of stencil onto target can be produced``()=
        let target = example.Figure.a_high_level_relatively_simple_figure
        let mappee = example.Figure.fitting_stencil_as_figure
        
        (map_figure_onto_target target mappee)
        |> should equal
            [
                Mapping.ofStringPairs [
                    "b#1","b#1";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
                Mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
            ]

            
    let ``paint the target figure and the stencil``()=
        let figure = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil

        "F"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_circle_vertices
        |>infrastructure.Graph.with_filled_vertex "target figure" 
            (painted.Graph.add_graph figure.edges)
        |>infrastructure.Graph.with_filled_vertex "stencil"
            (painted.Graph.add_graph stencil.edges)
        |>painted.image.open_image_of_graph

    [<Fact>]
    let ``apply stencil to a long sequence``()=
        let middle_digit_stencil =
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        middle_digit_stencil
        |>results_of_stencil_application history_as_figure
        |>Set.ofSeq
        |>should equal (
            "12345678"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )
        
    [<Fact>]
    let ``apply stencil, when some first mapped vertices whould be skipped``()=
        let last_digit_stencil =
            {
                built.Stencil.simple_with_separator [
                    "N","out";
                    ",#1","out";
                    "out",";";
                ] with
                    output_without=
                        ","|>built.Figure.signal|>Set.singleton
            }
        
        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        last_digit_stencil
        |>results_of_stencil_application history_as_figure
        |>should equal (
            "9"
            |>built.Figure.signal
            |>Seq.singleton
        )

    