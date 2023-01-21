namespace rvinowise.ai.math

open Xunit
open FsUnit

open rvinowise.ai

module Math_primers=

    let 

    [<Test>]
    let input_primers=
        built.Event_batches.from_text_blocks 0 [
                ["N:0,1,2,3,4,5,6,7,8,9;"];
                ["1+2=3;×"];
                ["1+2="];
            ]