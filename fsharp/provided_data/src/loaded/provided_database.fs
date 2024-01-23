namespace rvinowise.ai.database

open System
open System.Data.Common
open System.Configuration
open Npgsql

module Provided=

    let connection_string = ""
    
    let open_connection =
        let data_source = NpgsqlDataSource.Create(connection_string)
        let db_connection = data_source.OpenConnection()
        
        db_connection