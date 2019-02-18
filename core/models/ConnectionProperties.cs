using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.models {
  public class ConnectionProperties {

    public string Action { get; set; }
    public string ConnectionString { get; set; }
    public string CommandText { get; set; }
    public IEnumerable<KeyValuePair<string, string>> Parameters { get; set; }

    public string Value { get; set; }
    public int? NumRecords { get; set; }
    public int? NumFields { get; set; }
    public int? Depth { get; set; }

  }
}
