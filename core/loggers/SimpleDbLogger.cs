using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shivs.utilities.core.models;

namespace shivs.utilities.core.loggers {
  public class SimpleDbLogger : IDbLogger {

    private string _filePath;


    private Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();
    private List<LogRecord> _records = new List<LogRecord>();

    public void endTimer(string timerId) {
      if (this._timers.ContainsKey(timerId)) {
        this._timers[timerId].EndTime = DateTime.Now;
      }
    }

    public void logClose(string connectionId, string connectionString) {
      this._records.Add(new LogRecord() {
        ConnectionId = connectionId,
        TimeExecuted = DateTime.Now,
        Properties = new ConnectionProperties() {
          Action = "Close",
          ConnectionString = connectionString,
        }
      });
    }

    public void logException(string connectionId, Exception ex, ConnectionProperties properties) {
      this._records.Add(new LogRecord() {
        ConnectionId = connectionId,
        Exception = ex,
        TimeExecuted = DateTime.Now,
        Properties = properties
      });

    }

    public void logOpen(string connectionId, string connectionString) {
      this._records.Add(new LogRecord() {
        ConnectionId = connectionId,
        TimeExecuted = DateTime.Now,
        Properties = new ConnectionProperties() {
          Action = "Open",
          ConnectionString = connectionString,
        }
      });

    }

    public void logTimer(string connectionId, string timerId, ConnectionProperties properties) {
      if (this._timers.ContainsKey(timerId)) {
        this._records.Add(new LogRecord() {
          ConnectionId = connectionId,
          TimeExecuted = this._timers[timerId].StartTime,
          Duration = (int)this._timers[timerId].EndTime.Subtract(this._timers[timerId].StartTime).TotalMilliseconds,
          Properties = properties
        });

      }
    }

    public string startTimer() {
      string returnValue = Guid.NewGuid().ToString().Replace("-", "");

      this._timers.Add(returnValue, new Timer() { StartTime = DateTime.Now });

      return returnValue;
    }

    public void writeToFile() {
      List<LogRecord> copy = new List<LogRecord>(this._records);
      this._records = new List<LogRecord>();

      List<string> lines = new List<string>();
      foreach(LogRecord r in copy) {
        if (null != r.Exception) {
          lines.Add("****ERROR****");
        }
        lines.Add("ConnectionId: " + r.ConnectionId);
        lines.Add("Time: " + r.TimeExecuted.ToShortTimeString());
        lines.Add("ConnectionString: " + r.Properties.ConnectionString);
        lines.Add("Action: " + r.Properties.Action);
        if (null != r.Properties.CommandText) {
          lines.Add("CommandText: " + r.Properties.CommandText);
        }
        if (null != r.Properties.Parameters && r.Properties.Parameters.Count() > 0) {
          lines.Add("Parameters: ");
          foreach (KeyValuePair<string, string> kvp in r.Properties.Parameters) {
            lines.Add("\t" + kvp.Key + " - " + kvp.Value);
          }
        }
        if (null != r.Exception) {
          lines.Add("Exception: " + r.Exception.Message);
        }
        else {
          if (r.Duration.HasValue) {
            lines.Add("Duration: " + r.Duration.Value);
          }
          if (null != r.Properties.Value) {
            lines.Add("Value: " + r.Properties.Value);
          }
          if (r.Properties.NumRecords.HasValue) {
            lines.Add("NumRecords: " + r.Properties.NumRecords.Value);
          }
          if (r.Properties.NumFields.HasValue) {
            lines.Add("NumFields: " + r.Properties.NumFields.Value);
          }
          if (r.Properties.Depth.HasValue) {
            lines.Add("Depth: " + r.Properties.Depth.Value);
          }
        }
        
        lines.Add(" ");

      }

      try {
        System.IO.File.AppendAllLines(this._filePath, lines);
      }
      catch {
        //TODO: We don't want this to get in the way, Find a safe place to log this exception
      }
    }

    public SimpleDbLogger(string filepath) {
      this._filePath = filepath;
    }
  }
}
