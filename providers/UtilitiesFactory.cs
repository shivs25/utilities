using shivs.utilities.core.connections;
using shivs.utilities.core.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.providers {
  public static class UtilitiesFactory {

    public static IDbConnection createLoggedConnection(IDbConnection connection, string directory) {
      if (!System.IO.Directory.Exists(directory)) {
        System.IO.Directory.CreateDirectory(directory);
      }
      string filepath = directory.TrimEnd("\\/".ToCharArray()) + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

      return new connections.NpgsqlLoggedConnection(connection, new SimpleDbLogger(filepath));
    }
  }
}
