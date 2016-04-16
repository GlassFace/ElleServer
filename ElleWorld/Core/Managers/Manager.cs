using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Managers
{
    class Manager
    {
        public static SessionManager Session;

        public static void Initialize()
        {
            Session = SessionManager.GetInstance();
        }
    }
}
