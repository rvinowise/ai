module rvinowise.ai.built.Figure_appearances
    open Xunit
    open FsUnit
    open rvinowise.ai

    let from_figure_id_appearances
        (figure_id_appearances: Figure_id_appearances)
        =
        {
            Figure_appearances.figure=
                built.Figure.signal figure_id_appearances.figure
            appearances=figure_id_appearances.appearances
        }
    
    let to_figure_id_appearances 
        (figure_appearances:Figure_appearances) 
        =
        {
            Figure_id_appearances.figure=
                Figure.id_of_a_sequence figure_appearances.figure
            appearances=figure_appearances.appearances
        }