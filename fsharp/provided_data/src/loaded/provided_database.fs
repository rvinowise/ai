﻿module rvinowise.ai.database.Provided

open System
open System.Data.Common
open System.Configuration


let connection_setting =
    try
        ConfigurationManager.ConnectionStrings.[3]
    with
    | :? Xml.XmlException as e ->
        reraise()
        

let factory = 

    DbProviderFactories.RegisterFactory(
        "System.Data.SqlClient", 
        System.Data.SqlClient.SqlClientFactory.Instance
    )

    DbProviderFactories.RegisterFactory(
        "System.Data.SQLite", 
        System.Data.SQLite.SQLiteFactory.Instance
    )

    DbProviderFactories.RegisterFactory(
        "MySqlConnector", MySqlConnector.MySqlConnectorFactory.Instance
    )

    DbProviderFactories.GetFactory(
        connection_setting.ProviderName
    )


let open_connection =
    
    let db_connection = factory.CreateConnection()
    db_connection.ConnectionString <- connection_setting.ConnectionString
    db_connection