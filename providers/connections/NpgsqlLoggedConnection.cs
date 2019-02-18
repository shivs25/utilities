using shivs.utilities.core.connections;
using shivs.utilities.core.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.providers.connections {
  public class NpgsqlLoggedConnection:LoggedConnection {

    protected override void clearPool() {
     if (this._connection is Npgsql.NpgsqlConnection) {
        Npgsql.NpgsqlConnection.ClearPool((Npgsql.NpgsqlConnection)this._connection);
     }
    }

    public NpgsqlLoggedConnection(IDbConnection connection, IDbLogger logger):base(connection, logger) {

    }
  }
}
