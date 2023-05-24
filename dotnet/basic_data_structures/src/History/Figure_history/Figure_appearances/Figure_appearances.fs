namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit


    type Figure_appearances = {
        figure: Figure
        appearances: array<Interval>
    } with
        override this.ToString() =
            let str_figure = string this.figure
            let str_appearances = Interval.intervals_to_string this.appearances
            $"""Figure_appearances({str_figure} appearances={str_appearances})"""
                

    module Appearances =
        
        let has_repetitions (appearances: Interval seq) =
            Seq.length appearances > 1
        
        