namespace rvinowise.ai

open Xunit
open FsUnit
open rvinowise
open rvinowise.ai.built
open rvinowise.ai.built.Figure
    
module Figure_tests=


    [<Fact>]
    let ``equality comparison``()=
        let f1 = ai.built.Figure.from_tuples [
            "a0","a","b0","b"
        ]
        let f2 = ai.built.Figure.from_tuples [
            "a1","a","b20","b"
        ]
        f1 |>should equal f2


    

    [<Fact>]
    let ``try id_from_sequence``()=
        ["a1","b";"b","a2";"a2","c"]
        |>built.Figure.simple_without_separator
        |>ai.Figure.id_of_a_sequence
        |>should equal (Figure_id "abac")
    
    
    [<Fact>]
    let ``try id_from_sequence for a signal``()=
        "a"
        |>built.Figure.signal
        |>ai.Figure.id_of_a_sequence
        |>should equal (Figure_id "a")
    

    [<Fact>]
    let ``try is_sequence``()=
        "abcde"
        |>built.Figure.sequential_figure_from_text
        |>(fun f->f.edges)
        |>Edges.is_sequence
        |>should equal true

        [
            "a","b";"b","x1";
                    "b","y1";"x1","c";
                             "y1","c"]
        |>built.Figure.simple_without_separator
        |>(fun f->f.edges)
        |>Edges.is_sequence
        |>should equal false

    
    
    [<Fact>]
    let ``detect cycles in figures``()=
        Assert.Throws<BadGraph> (fun()->
            example.Figure.create_a_bad_figure_with_cycle()
            |>ignore
        )
    
    [<Fact>]
    let ``try rename_vertices_to_standard_names``()=
        built.Figure.from_tuples [
            "my_a0","a","my_b1","b";
            "my_a0","a","uppercase_b","B";
            "uppercase_b","B","c0_at_the_end","figure_c";
            "uppercase_b","B","another_a","a";
        ]|>Renaming_figures.rename_vertices_to_standard_names
        |>built.Figure.check_correctness
        |>should equal (
            built.Figure.from_tuples [
                "a1","a","b1","b";
                "a1","a","B1","B";
                "B1","B","figure_c1","figure_c";
                "B1","B","a2","a";
            ]|>Renaming_figures.rename_vertices_to_standard_names
        )
    
    [<Fact>]
    let ``standartizing names allows for structural comparison of figures``()=
        let figure1 = {
            Figure.edges=[
                "a1","b1";
                "a2","b1";
                "a3","c1";
                "b1","d1";
                "c1","d1";
                "c1","d2";
            ]|>List.map Edge.ofStringPair
            |>Set.ofList
            subfigures=[
                "a1","a";
                "a2","a";
                "a3","a";
                "b1","b";
                "c1","c";
                "d1","d";
                "d2","d";
            ]
            |>List.map (fun pair->
                pair|>fst|>Vertex_id,
                pair|>snd|>Figure_id|>built.Subfigure.referencing_constant_figure
            )
            |>Map.ofList
        }
        let figure2 = {
            Figure.edges=[
                "a1","b1";
                "a3","b1";
                "a2","c1";
                "b1","d2";
                "c1","d2";
                "c1","d1";
            ]|>List.map Edge.ofStringPair
            |>Set.ofList
            subfigures=[
                "a1","a";
                "a2","a";
                "a3","a";
                "b1","b";
                "c1","c";
                "d1","d";
                "d2","d";
            ]
            |>List.map (fun pair->
                pair|>fst|>Vertex_id,
                pair|>snd|>Figure_id|>built.Subfigure.referencing_constant_figure
            )
            |>Map.ofList
        }
        figure1
        |>Renaming_figures.rename_vertices_to_standard_names
        |>should equal (
            figure2
            |>Renaming_figures.rename_vertices_to_standard_names
        )