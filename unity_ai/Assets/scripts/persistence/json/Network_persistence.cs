using System.IO;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using SimpleFileBrowser;
using UnityEngine;


namespace rvinowise.ai.unity.persistence {
public class Network_persistence<TFigure>: 
    MonoBehaviour 
    where TFigure: class?, IFigure 
{

    [HideInInspector] public string saving_path;


    [SerializeField] public Action_history action_history;
    public Figure_provider<TFigure> figure_provider;
    
    void Awake() {
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