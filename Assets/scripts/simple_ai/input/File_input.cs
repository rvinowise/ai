using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using SimpleFileBrowser;

using System.IO.Abstractions;

namespace rvinowise.ai.simple {


public class File_input<TFigure>
    where TFigure: class?, IFigure 
{
    private readonly IInput_receiver receiver;
    private readonly IFigure_provider<TFigure> figure_provider;
    private readonly IFileSystem file_system;

    public File_input(
        IInput_receiver receiver,
        IFigure_provider<TFigure> figure_provider,
        IFileSystem file_system
    ) {
        this.receiver = receiver;
        this.figure_provider = figure_provider;
        this.file_system = file_system;
    }

    public File_input(
        IInput_receiver receiver,
        IFigure_provider<TFigure> figure_provider
    ): this (
        receiver,
        figure_provider,
        new FileSystem()
    ) { }


    public void read_file(string file_path) {
        string input_string = file_system.File.ReadAllText(file_path);
        foreach(char symbol in input_string) {
            receiver.input_signals(
                new []{figure_provider.provide_figure(symbol.ToString())}
            );
        }  
    }

}


}
