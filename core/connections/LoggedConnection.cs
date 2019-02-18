using shivs.utilities.core.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.connections {
  public class LoggedConnection : IDbConnection {


    protected IDbConnection _connection;

    private IDbLogger _logger;
    private string _connectionId;

    public string ConnectionString { get => this._connection.ConnectionString; set => this._connection.ConnectionString = value; }

    public int ConnectionTimeout => this._connection.ConnectionTimeout;

    public string Database => this._connection.Database;

    public ConnectionState State => this._connection.State;

    public IDbTransaction BeginTransaction() {
      return this._connection.BeginTransaction();
    }

    public IDbTransaction BeginTransaction(IsolationLevel il) {
      return this._connection.BeginTransaction(il);
    }

    public void ChangeDatabase(string databaseName) {
      this._connection.ChangeDatabase(databaseName);
    }

    public void Close() {
      this._connection.Close();
      this._logger.logClose(this._connectionId, this._connection.ConnectionString);
      this._logger.writeToFile();
    }

    public IDbCommand CreateCommand() {
      return new LoggedCommand(this._connectionId, this._connection.CreateCommand(), this._logger);
    }

    public void Dispose() {
      this._connection.Dispose();
    }

    protected virtual void clearPool() {

    }

    public void Open() {
      try {
        this._connectionId = Guid.NewGuid().ToString().Replace("-", "");
        this._connection.Open();
        this._logger.logOpen(this._connectionId, this._connection.ConnectionString);
      }
      catch(Exception ex) {
        this._logger.logException(this._connectionId, ex, new models.ConnectionProperties() {
          ConnectionString = this._connection.ConnectionString,
          Action = "Open"
        });

        throw ex;
      }
     
    }

    public LoggedConnection(IDbConnection connection, IDbLogger logger) {
      this._connection = connection;
      this._logger = logger;
    }
  }
}
