using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;
using rvinowise.ai.general;
using rvinowise.ai.unity;
using rvinowise.ai.unity.persistence.serializable;
using SimpleFileBrowser;

namespace rvinowise.ai.unity.persistence {
public class Network_saver:
    MonoBehaviour 
{
    
    string saving_path;
    public Network_persistence persistence;
    
    [SerializeField] Action_history action_history;
    [SerializeField] Pattern_storage pattern_storage;
    [SerializeField] Figure_storage figure_storage;

    public static Network_saver instance;

    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
        
    }
    
    private void init_file_dialog() {
        FileBrowser.SetFilters( 
            true, 
            new FileBrowser.Filter( ".json", ".json") 
        );
        FileBrowser.SetDefaultFilter( ".json" );
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
        FileBrowser.AddQuickLink( "Savings", saving_path);
    }
    
    public void on_btn_open_file() {
        init_file_dialog();
        FileBrowser.ShowSaveDialog( 
            on_save,
            null, 
            FileBrowser.PickMode.Files
        );
    }
    
    public void on_save(string[] files) {
        on_save(files[0]);
    }
    
    public void on_save(string file) {
        serializable.Network out_network = create_serializable_network();
        write_json(file, out_network);
    }

    public serializable.Network create_serializable_network() {
        serializable.Network out_network = new serializable.Network();
        serialize_action_groups(out_network);
        serialize_figures(out_network);
        return out_network;
    }

    private void serialize_action_groups(serializable.Network out_network) {
        foreach(
            IAction_group action_group in 
            action_history.get_action_groups(
                0, action_history.last_moment
            )
        ) {
            out_network.action_groups.Add(
                new serializable.Action_group(action_group)
            );
        }
    }
    
    private void serialize_figures(serializable.Network out_network) {
        foreach(
            Figure figure in 
            figure_storage.known_figures
        ) {
            out_network.figures.Add(
                new serializable.Figure(figure)
            );
            serialize_figure_appearances(out_network, figure);
        }
    }
    

    private void serialize_figure_appearances(
        serializable.Network out_network,
        general.IFigure figure
    ) {
        foreach(
            IFigure_appearance appearance in 
            figure.get_appearances_in_interval(
                0,
                action_history.last_moment
            )
        ) {
            out_network.figure_appearances.Add(
                new serializable.Figure_appearance(appearance)
            );
        }
    }
    
    private void write_json<T>(string path, T obj) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, persistence.json_setting);
        File.WriteAllText(path, json);
    }
}
}