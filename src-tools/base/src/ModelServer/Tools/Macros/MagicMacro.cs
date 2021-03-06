using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Generator;
using Example.Project.Tools.Templates.Scaffold;

namespace Example.Project.Tools.Macros
{
    public class MagicMacro : ICommand
    {
        #region ICommand Members

        public void Execute()
        {
            new PrepareMacro().Execute();
            new ScaffoldGenerator().Execute();
        }

        #endregion
    }
}
