using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.connections {
  public interface ILoggedConnection {
    bool IsNpgSqlFunction();
    TextReader BeginTextExport(string command);
  }
}
