using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using SimpleFileBrowser;
using UnityEngine;


namespace rvinowise.ai.unity.persistence {
public class Network_persistence: 
    MonoBehaviour {

    [HideInInspector] public string saving_path;
    [HideInInspector] public JsonSerializerSettings json_setting = new JsonSerializerSettings
        {TypeNameHandling = TypeNameHandling.None};

    
    [SerializeField] public Action_history action_history;
    [SerializeField] public Pattern_storage pattern_storage;
    [SerializeField] public Figure_storage figure_storage;
    
    public static Network_persistence instance;
    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
        //saving_path = Application.dataPath+"/savings";
        saving_path = Application.dataPath + "/savings";
        Directory.CreateDirectory(saving_path);
        init_file_dialog();
    }

    private void init_file_dialog() {
        FileBrowser.SetFilters( 
            true, 
            new FileBrowser.Filter( "Network Json Files", ".json") 
        );
        FileBrowser.SetDefaultFilter( ".json" );
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
        FileBrowser.AddQuickLink( "Savings", saving_path);
    }
    
    public string get_filename_for_saving() {
        return saving_path + "/network.json";
    }
    public string get_filename_for_loading() {
        return saving_path + "/network.json";
    }


  
}
}