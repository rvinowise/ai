namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit

    type Figure_appearances = {
        figure: Figure
        appearances: array<Interval>
    }

    module Figure_appearances =
        
        let has_repetitions history =
            Seq.length history.appearances > 1
        
        