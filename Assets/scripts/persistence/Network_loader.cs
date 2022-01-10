using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }

    private void load_patterns(List<serializable.Pattern> patterns) {
        pattern_storage = persistence.pattern_storage;
        foreach (serializable.Pattern pattern in patterns) {
            IPattern new_pattern;
            if (pattern.subfigures.Any()) {
                new_pattern = pattern_storage.pattern_prefab.get_for_base_input(pattern.id);
            }
            else {
                new_pattern = get_pattern_having(pattern.subfigures);
            }
            pattern_storage.append_pattern(new_pattern);
        }
    }
    private IPattern get_pattern_having(IList<string> subfigures_ids) {
        List<IFigure> subfigures = new List<IFigure>();
        foreach (string subfigure_id in subfigures_ids) {
            subfigures.Add(pattern_storage.get_pattern_for_base_input(subfigure_id));
        }
        return pattern_storage.get_pattern_having(subfigures);
    }

    private T read_json<T>(string path) {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, persistence.json_setting);
    }

  
}
}