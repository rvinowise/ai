namespace rvinowise.ai

open System.Collections.Generic
open rvinowise
open FsUnit
open Xunit
open System
open System.Text
open System.Diagnostics.Contracts


module Sequence_printing =
    let sequence_to_string
        sequence
        =
        sequence
        |>Seq.map Figure_id.value
        |>String.Concat


type Sequence_appearances = {
    sequence: Figure_id array
    appearances: Interval array
} with
    override this.ToString()=
        let str_sequence=
            this.sequence
            |>Sequence_printing.sequence_to_string
            
        let str_appearances = Interval.intervals_to_string this.appearances
        $"""{str_sequence} appearances={str_appearances}"""


    
        