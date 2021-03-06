﻿using shivs.utilities.core.connections;
using shivs.utilities.core.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.providers.connections {
  public class NpgsqlLoggedConnection:LoggedConnection, ITextExportConnection {

    public TextReader beginTextExport(string command) {
      if (this._connection is Npgsql.NpgsqlConnection) {
        return ((Npgsql.NpgsqlConnection)_connection).BeginTextExport(command);
      }
      else {
        throw new NotImplementedException();
      }
    }


    public TextWriter beginTextImport(string command) {
      if (this._connection is Npgsql.NpgsqlConnection) {
        return ((Npgsql.NpgsqlConnection)_connection).BeginTextImport(command);
      }
      else {
        throw new NotImplementedException();
      }
    }



    protected override void clearPool() {
     if (this._connection is Npgsql.NpgsqlConnection) {
        Npgsql.NpgsqlConnection.ClearPool((Npgsql.NpgsqlConnection)this._connection);
     }
    }

    public NpgsqlLoggedConnection(IDbConnection connection, IDbLogger logger):base(connection, logger) {

    }
  }
}
