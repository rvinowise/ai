﻿namespace rvinowise.ai
    open FsUnit
    open Xunit
    
    open rvinowise.ai

    type Appearance_event=
    |Start of Figure_id
    |Finish of Figure_id * Moment
    |Signal of Figure_id

    

        