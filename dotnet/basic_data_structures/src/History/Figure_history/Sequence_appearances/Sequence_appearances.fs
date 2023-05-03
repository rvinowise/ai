namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit


    type Sequence_appearances = {
        sequence: Figure_id array
        appearances: array<Interval>
    } with
        override this.ToString()=
            printed.Sequence_appearances.to_string this.sequence this.appearances

  
        
        