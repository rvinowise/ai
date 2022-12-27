namespace rvinowise.ai

    module Vertex =

        open System.Text.RegularExpressions
        let remove_number label =
            Regex.Replace(label, @"[^a-zA-Z]", "")


        