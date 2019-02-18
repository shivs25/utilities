using shivs.utilities.core.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.connections {
  public class LoggedCommand : IDbCommand {

    private string _connectionId;
    private IDbLogger _logger;
    private IDbCommand _command;

    public IDbConnection Connection { get => this._command.Connection; set => this._command.Connection = value; }
    public IDbTransaction Transaction { get => this._command.Transaction; set => this._command.Transaction = value; }
    public string CommandText { get => this._command.CommandText; set => this._command.CommandText = value; }
    public int CommandTimeout { get => this._command.CommandTimeout; set => this._command.CommandTimeout = value; }
    public CommandType CommandType { get => this._command.CommandType; set => this._command.CommandType = value; }

    public IDataParameterCollection Parameters => this._command.Parameters;

    public UpdateRowSource UpdatedRowSource { get => this._command.UpdatedRowSource; set => this._command.UpdatedRowSource = value; }

    public void Cancel() {
      this._command.Cancel();
    }

    public IDbDataParameter CreateParameter() {
      return this._command.CreateParameter();
    }

    public void Dispose() {
      this._command.Dispose();
    }

    public int ExecuteNonQuery() {
      string id = this._logger.startTimer();

      int returnValue = this._command.ExecuteNonQuery();

      this._logger.endTimer(id);

      this._logger.logTimer(this._connectionId, id, new models.ConnectionProperties() {
        Action = "ExecuteNonQuery",
        CommandText = this.CommandText,
        ConnectionString = this._command.Connection.ConnectionString,
        NumFields = returnValue,
        Parameters = this.createParameterList()
      });

      return returnValue;
    }

    public IDataReader ExecuteReader() {
      string id = this._logger.startTimer();

      IDataReader returnValue = this._command.ExecuteReader();
      this._logger.endTimer(id);

      this._logger.logTimer(this._connectionId, id, new models.ConnectionProperties() {
        Action = "ExecuteReader",
        CommandText = this.CommandText,
        ConnectionString = this._command.Connection.ConnectionString,
        NumFields = returnValue.FieldCount,
        NumRecords = returnValue.RecordsAffected,
        Depth = returnValue.Depth,
        Parameters = this.createParameterList()
      });

      return returnValue;
    }

    public IDataReader ExecuteReader(CommandBehavior behavior) {
      string id = this._logger.startTimer();

      IDataReader returnValue = this._command.ExecuteReader();
      this._logger.endTimer(id);

      this._logger.logTimer(this._connectionId, id, new models.ConnectionProperties() {
        Action = "ExecuteReader(CommandBehavior)",
        CommandText = this.CommandText,
        ConnectionString = this._command.Connection.ConnectionString,
        NumFields = returnValue.FieldCount,
        NumRecords = returnValue.RecordsAffected,
        Depth = returnValue.Depth,
        Parameters = this.createParameterList()
      });

      return returnValue;
    }

    public object ExecuteScalar() {
      string id = this._logger.startTimer();

      object returnValue = this._command.ExecuteScalar();
      this._logger.endTimer(id);

      this._logger.logTimer(this._connectionId, id, new models.ConnectionProperties() {
        Action = "ExecuteScalar",
        CommandText = this.CommandText,
        ConnectionString = this._command.Connection.ConnectionString,
        NumFields = 1,
        NumRecords = 1,
        Value = returnValue.ToString(),
        Parameters = this.createParameterList()
      });

      return returnValue;
    }

    public void Prepare() {
      try {
        this._command.Prepare();
      }
      catch(Exception ex) {
        this._logger.logException(this._connectionId, ex, new models.ConnectionProperties() {
          ConnectionString = this._command.Connection.ConnectionString,
          Action = "Prepare",
          CommandText = this.CommandText,
          Parameters = this.createParameterList()
        });

        throw ex;
      }
    }

    private IEnumerable<KeyValuePair<string, string>> createParameterList() {
      List<KeyValuePair<string, string>> returnValue = new List<KeyValuePair<string, string>>();

      if (this.Parameters.Count > 0) {
        foreach(IDbDataParameter p in this.Parameters) {
          returnValue.Add(new KeyValuePair<string, string>(p.ParameterName, p.Value.ToString()));
        }
      }

      return returnValue;
    }

    public LoggedCommand(string connectionId, IDbCommand command, IDbLogger logger) {
      this._command = command;
      this._connectionId = connectionId;
      this._logger = logger;
    }
  }
}
