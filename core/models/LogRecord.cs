using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.models {
  public  class LogRecord {

    public string ConnectionId { get; set; }
    public DateTime TimeExecuted { get; set; }
    public int? Duration { get; set; }
    public ConnectionProperties Properties { get; set; }
    public Exception Exception { get; set; }
  }
}
