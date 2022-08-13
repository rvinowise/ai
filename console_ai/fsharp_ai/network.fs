module ai.network

type Figure_appearance = {
    start: int;
    ending: int;
}
type Subfigure = {
    id: string;
    parent_figure: Figure;
    represented_figure: Figure;
    previous: Subfigure list;
    next: Subfigure list;
}
type Figure = {
    id: string; 
    appearances: Figure_appearance list;
    subfigures: Subfigure list
}

type Figure_storage = {
    known_figures: Figure list;
}

let figure_storage: Figure_storage = {
    known_figures = [
        {
            id = "a";
            appearances = [
                {start=1;ending=3}
                {start=6;ending=7}
            ]
        }
        {
            id = "b";
            appearances = [
                {start=4;ending=5}
            ]
        }
        {
            id = "c";
            appearances = []
        }
    ]
}

let get_figure figure_id figure_storage =
    figure_storage.known_figures
    |> Seq.tryFind(fun figure -> figure.id = figure_id)

let print_figure_appearance appearance =
    printf $"({appearance.start} {appearance.ending}) "

let print_appearances_of (figure: Figure) =
    printfn "appearances:"
    figure.appearances
    |> Seq.iter print_figure_appearance

let print_figure (figure: Figure) =
    printfn "id: %s" figure.id
    print_appearances_of figure

