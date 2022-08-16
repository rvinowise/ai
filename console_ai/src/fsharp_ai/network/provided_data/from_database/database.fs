module rvinowise.ai.database.Provided

open System.Data.Common
open System.Configuration




//let init_connection _ =
let connection_setting = ConfigurationManager.ConnectionStrings.[1]
    
let factory = 

    DbProviderFactories.RegisterFactory(
        "System.Data.SqlClient", 
        System.Data.SqlClient.SqlClientFactory.Instance
    )

    DbProviderFactories.RegisterFactory(
        "System.Data.SQLite", 
        System.Data.SQLite.SQLiteFactory.Instance
    )
    printf "registering a factory"

    DbProviderFactories.GetFactory(
        connection_setting.ProviderName
    )

let open_connection =
    
    let db_connection = factory.CreateConnection()
    db_connection.ConnectionString <- connection_setting.ConnectionString
    db_connection