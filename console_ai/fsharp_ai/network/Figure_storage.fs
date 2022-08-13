namespace rvinowise.ai



    //        {
    //            id = "a";
    //            appearances = [
    //                {start=1;ending=3}
    //                {start=6;ending=7}
    //            ]
    //        }
    //        {
    //            id = "b";
    //            appearances = [
    //                {start=4;ending=5}
    //            ]
    //        }
    //        {
    //            id = "c";
    //            appearances = []
    //        }
    //    ]
    }

    let get_figure figure_id figure_storage =
        figure_storage.known_figures
        |> Seq.tryFind(fun figure -> figure.id = figure_id)