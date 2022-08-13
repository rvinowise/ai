namespace rvinowise.unity.ui.table {

public interface ITable<TItem> {


    public TItem add_item(TItem in_item);

    public void remove_item(TItem in_item);
}
} 