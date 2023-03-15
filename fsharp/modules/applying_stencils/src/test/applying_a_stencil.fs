namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise.ai.Applying_stencil
open rvinowise.ai.ui
open rvinowise.ai.mapping_stencils


        
module ``application of stencils``=
    
    open rvinowise.ai.stencil
    open rvinowise.ui
    
        
    let initial_mapping_without_prolongation = Mapping.ofStringPairs ["b","b1";"h","h"]
    let initial_fruitful_mapping = Mapping.ofStringPairs ["b","b0";"h","h"]
    let initial_useless_mapping = Mapping.ofStringPairs ["b","b2";"h","h"]
        
    let result_of_fruitful_stencil_application =
        built.Figure.simple
            [
                "d","e"
            ]
              

    [<Fact>]
    let ``impossible to prolongate, because of no matching following subfigures``()=
        prolongate_one_mapping_with_next_subfigures 
            example.Stencil.a_fitting_stencil
            example.Figure.a_high_level_relatively_simple_figure
            [(Vertex_id "f", Figure_id "f")]
            initial_mapping_without_prolongation
        |> should equal
            []
    
    [<Fact>]
    let``prolongation with a single following node``()=
        prolongate_one_mapping_with_next_subfigures 
            example.Stencil.a_fitting_stencil
            example.Figure.a_high_level_relatively_simple_figure
            [(Vertex_id "f", Figure_id "f")]
            initial_useless_mapping
        |> should equal
            [
                Mapping.ofStringPairs [
                    "b","b2";
                    "h","h";
                    "f","f1"
                ]
            ]
    

    
    [<Fact>]
    let ``several mappings can be produced by one stencil application``()=
        let result =
            map_stencil_onto_target
                example.Stencil.a_fitting_stencil
                example.Figure.a_high_level_relatively_simple_figure
                |> Set.ofSeq
        let expected =
            (Set.ofSeq [
                Mapping.ofStringPairs [
                    "b","b0";
                    "h","h";
                    "f","f1"
                ];
                Mapping.ofStringPairs [
                    "b","b2";
                    "h","h";
                    "f","f1"
                ]
            ])
        ()

    [<Fact>]
    let ``a full mapping can be produced if the stencil has only "out" in the middle``()=
        let stencil = 
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]

        let target =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequence_from_text
   
        let mappings =
            map_stencil_onto_target
                stencil
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
    let ``a fitting stencil, applied to a figure, outputs subgraphs``()=
        let target = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil
        stencil
        |>results_of_stencil_application target
        |>should equal [
            built.Figure.simple
                [
                    "d","e"
                ]
        ]


    [<Fact>]
    let ``preparing inputs for permutators, which map initial nodes``()=
        
        let subfigures_in_stencil, subfigures_in_target =
            sorted_subfigures_to_map_first
                example.Stencil.a_fitting_stencil
                example.Figure.a_high_level_relatively_simple_figure
        
        let permutator_input = 
            input_for_first_mappings_permutators 
                subfigures_in_stencil
                subfigures_in_target
                                
        permutator_input |> should equal [|
            (* b *)(1,3) ;
            (* h *)(1,1) ;
        |]

    [<Fact>]
    let ``generator of initial mappings returns combinations``()=
        
        let generator = Generator_of_orders<int[]>()
    
        generator.add_order(Generator_of_mappings(1, 3))
        let enumerator = generator.GetEnumerator()
        enumerator.MoveNext()
        |> should equal true
        enumerator.Current
        |> should equal [| [|0|] |]
        enumerator.MoveNext()
        |> should equal true
        enumerator.Current
        |> should equal [| [|1|] |]
        enumerator.MoveNext()
        |> should equal true
        enumerator.Current
        |> should equal [| [|2|] |]
        enumerator.MoveNext()
        |> should equal false

    
    [<Fact>]
    let ``mapping of first stencil subfigures onto target produces initial mapping``()=
        let figure = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil
        
        (
            map_first_nodes 
                stencil
                figure
        )
        |> should equal
            [   Mapping.ofStringPairs ["b","b0";"h","h"];
                Mapping.ofStringPairs ["b","b1";"h","h"];
                Mapping.ofStringPairs ["b","b2";"h","h"]
            ]
        
    [<Fact>]
    let ``finding following subfigures referencing a specific figure``()=
        (Figure.subfigures_after_other_subfigures
            example.Figure.a_high_level_relatively_simple_figure
            (Figure_id "f")
            [Vertex_id "b0"]
        )|> should equal
            [Vertex_id "f0";Vertex_id "f1"]

        (Figure.subfigures_after_other_subfigures
            example.Figure.a_high_level_relatively_simple_figure
            (Figure_id "f")
            [Vertex_id "d";Vertex_id "b2"]
        )|> should equal
            [Vertex_id "f1"]
    
    [<Fact>]
    let ``complete mapping of stencil onto target can be produced``()=
        let figure = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil
        
        (map_stencil_onto_target stencil figure)
        |> should equal
            [
                Mapping.ofStringPairs [
                    "b","b0";
                    "h","h";
                    "f","f1"; 
                ];
                Mapping.ofStringPairs [
                    "b","b2";
                    "h","h";
                    "f","f1";
                ];
            ]
            //|>Seq.map Set.ofSeq
        


    [<Fact>] //(Skip="ui")
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
        let number_concept = 
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequence_from_text

        number_concept
        |>Applying_stencil.results_of_stencil_application history_as_figure
        |>Set.ofSeq
        |>Set.isSubset (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )|>should equal true

    