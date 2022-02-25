using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using SimpleFileBrowser;
using UnityEngine;


namespace rvinowise.ai.unity.persistence {
public class Network_loader: 
    MonoBehaviour
{
    public Network_persistence persistence;
    [SerializeField] private Figure_storage figure_storage;
    //[SerializeField] private 

    public static Network_loader instance;
    
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
        FileBrowser.AddQuickLink( "Savings", persistence.saving_path);
    }
    
    public void on_btn_open_file() {
        init_file_dialog();
        FileBrowser.ShowLoadDialog( 
            on_load,
            null, 
            FileBrowser.PickMode.Files
        );
    }
    
    public void on_load(string[] files) {
        on_load(files[0]);
    }
    public void on_load(string file) {
        serializable.Network in_network = read_json<serializable.Network>(file);
        reconstruct_network(in_network);
    }

    private void reconstruct_network(serializable.Network network) {
        //load_figures(network.figures);
        load_action_groups(network.action_groups);
        load_figure_appearances(network.figure_appearances);
    }

    #region figures
    /*private void load_figures(List<serializable.Figure> figures) {
        figure_storage = persistence.figure_storage;
        foreach (serializable.Figure figure in figures) {
            if (figure.subfigures.Any()) {
                create_figure_having(figure.subfigures);
            }
            else {
                figure_storage.provide_pattern_for_base_input(pattern.id);
            }
        }
    }
    private void create_figure_having(IList<string> subfigures_ids) {
        List<IFigure> subfigures = new List<IFigure>();
        foreach (string subfigure_id in subfigures_ids) {
            subfigures.Add(figure_storage.provide_pattern_for_base_input(subfigure_id));
        }
        figure_storage.provide_pattern_having(subfigures);
    }*/
    #endregion figures
    
    #region action_groups

    private void load_action_groups( IList<serializable.Action_group> action_groups) {
        Action_history history = persistence.action_history;
        foreach (serializable.Action_group group in action_groups) {
            Action_group new_group = 
                history.create_next_action_group(group.mood);
            new_group.transform.position = group.position.to_unity();
        }
    }
    
    #endregion
    
    #region figure_appearances

    private void load_figure_appearances(IList<serializable.Figure_appearance> appearances) {
        Action_history history = persistence.action_history;
        Figure_storage storage = persistence.figure_storage;
        foreach (serializable.Figure_appearance appearance in appearances) {
            IFigure figure = storage.find_figure_with_id(
                appearance.appeared_figure
            );
            Contract.Ensures(
                figure != null, 
                "figures should be loaded before their appearances"
            );
            history.create_figure_appearance(
                figure,
                history.get_action_group_at_moment(appearance.get_start_moment()),
                history.get_action_group_at_moment(appearance.get_end_moment())
            );
        }
    }
    #endregion figure_appearances

    private T read_json<T>(string path) {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, persistence.json_setting);
    }

  
}
}