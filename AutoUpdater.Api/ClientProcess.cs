using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTeco.Lib.Base;
using iTeco.Lib.Srv;

namespace AutoUpdater.Api
{
    public class ClientProcess: ServiceProcessBase
    {
        protected override void DoWork(object p)
        {
            base.DoWork(p);
        }

        protected override void DoError(Exception err, object p)
        {
            base.DoError(err, p);
        }
    }
}
