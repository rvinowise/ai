namespace rvinowise.ai.built
    
open Xunit
open FsUnit


open rvinowise.ai
open rvinowise.extensions



module Subfigure=
    
    let does_subfigure_reference_needed_figure needed_figure target_subfigure =
            target_subfigure.name = needed_figure
    let referencing_constant_figure figure =
        {
            Subfigure.name = figure
            is_mappable = does_subfigure_reference_needed_figure figure
        }
        
    let referencing_anything_except figure =
        {
            Subfigure.name = figure
            is_mappable = does_subfigure_reference_needed_figure figure >> not
        }