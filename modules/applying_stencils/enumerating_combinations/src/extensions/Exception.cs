namespace rvinowise.contracts {
public class Broken_contract_exception: System.Exception {
    public string message = "";
    public Broken_contract_exception()
    {
    }

    public Broken_contract_exception(string message) : this()
    {
        this.message = message;
    }
}
}