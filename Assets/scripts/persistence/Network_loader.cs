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
    private Pattern_storage pattern_storage;

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
        load_patterns(network.patterns);
        load_action_groups(network.action_groups);
        load_figure_appearances(network.figure_appearances);
    }

    #region patterns
    private void load_patterns(List<serializable.Pattern> patterns) {
        pattern_storage = persistence.pattern_storage;
        foreach (serializable.Pattern pattern in patterns) {
            if (pattern.subfigures.Any()) {
                create_pattern_having(pattern.subfigures);
            }
            else {
                pattern_storage.provide_pattern_for_base_input(pattern.id);
            }
        }
    }
    private void create_pattern_having(IList<string> subfigures_ids) {
        List<IFigure> subfigures = new List<IFigure>();
        foreach (string subfigure_id in subfigures_ids) {
            subfigures.Add(pattern_storage.provide_pattern_for_base_input(subfigure_id));
        }
        pattern_storage.provide_pattern_having(subfigures);
    }
    #endregion patterns
    
    #region action_groups

    private void load_action_groups( IList<serializable.Action_group> action_groups) {
        Action_history history = persistence.action_history;
        foreach (serializable.Action_group group in action_groups) {
            history.actio
        }
    }
    
    #endregion
    
    #region figure_appearances

    private void load_figure_appearances(IList<serializable.Figure_appearance> appearances) {
        Action_history history = persistence.action_history;
        Pattern_storage storage = persistence.pattern_storage;
        foreach (serializable.Figure_appearance appearance in appearances) {
            IPattern pattern = storage.find_pattern_with_id(appearance.appeared_figure);
            Contract.Ensures(
                pattern != null, 
                "patterns should be loaded before their appearances"
            );
            history.create_pattern_appearance(
                pattern,
                appearance.get_start_moment(),
                appearance.get_end_moment()
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