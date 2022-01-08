using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using SimpleFileBrowser;

namespace rvinowise.ai.unity {


public class File_input : Input {
    
    
    private Dictionary<string, IPattern> name_to_pattern = 
        new Dictionary<string, IPattern>();


    override protected void Awake() {
        base.Awake();
    }
    protected void Start() {
        init_file_dialog();
        
    }

    private void init_file_dialog() {

		FileBrowser.SetFilters( true, 
            new FileBrowser.Filter( "Text Files", ".txt", ".csv" ) 
        );

		FileBrowser.SetDefaultFilter( ".txt" );
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		//StartCoroutine( ShowLoadDialogCoroutine() );
    }

    public void on_btn_open_file() {
        FileBrowser.ShowLoadDialog( 
            on_file_selected,
            on_cancel, 
            FileBrowser.PickMode.Files
        );
    }

    public void on_file_selected(string[] paths) {
        Contract.Requires(paths.Count() == 1);
        read_file(paths[0]);
    }

    public void on_cancel() {

    }
   
    public void read_file(string file_path) {
        string input_string = File.ReadAllText(file_path);
        foreach(char symbol in input_string.ToCharArray()) {
            if (symbol == '\n') {
                receiver.start_new_line();
            } else {
                pattern_storage.select_patterns_from_string(symbol.ToString());
                receiver.input_selected_patterns();
            }
        }  
    }

}
}
