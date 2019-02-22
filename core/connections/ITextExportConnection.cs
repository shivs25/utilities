using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shivs.utilities.core.connections {
  public interface ITextExportConnection {

    TextReader beginTextExport(string command);

  }
}
