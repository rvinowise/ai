using SimpleFileBrowser;


namespace rvinowise.ai.unity {


public class File_input : Input {
    
    
    private rvinowise.ai.simple.File_input<Figure> file_input;

    protected virtual void Awake() {
        base.Awake();
        file_input = new ai.simple.File_input<Figure>(receiver,figure_provider);
    }
    
    private void init_file_dialog() {
		FileBrowser.SetFilters( 
            true, 
            new FileBrowser.Filter( "Text Files", ".txt", ".csv" ) 
        );
		FileBrowser.SetDefaultFilter( ".txt" );
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Users", "C:\\Users" );
    }

    public void on_btn_open_file() {
        init_file_dialog();
        FileBrowser.ShowLoadDialog( 
            on_file_selected,
            null, 
            FileBrowser.PickMode.Files
        );
    }

    private void on_file_selected(string[] paths) {
        file_input.read_file(paths[0]);
    }

   

}
}
