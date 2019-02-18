using shivs.utilities.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.loggers {
  public interface IDbLogger {

    
    string startTimer();
    void endTimer(string timerId);
    void logTimer(string connectionId, string timerId, ConnectionProperties properties);

    void logOpen(string connectionId, string connectionString);
    void logClose(string connectionId, string connectionString);

    void logException(string connectionId, Exception ex, ConnectionProperties properties);

    void writeToFile();
  }
}
