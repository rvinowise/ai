namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit

    

    type Figure_appearances = {
        figure: Figure
        appearances: array<Interval>
    } with
        override this.ToString()=
            printed.Figure_appearances.to_string this.figure this.appearances
        

    module Appearances =
        
        let has_repetitions (appearances: Interval seq) =
            Seq.length appearances > 1
        
        