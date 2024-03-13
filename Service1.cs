using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MonitorGuaba
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CompileShell cs=new CompileShell();
            cs.Main(@"C:\ScriptShell\", "https://comunication.app.guabastudio.com/api/messages", "error");
        }

        protected override void OnStop()
        {
            // Puedes agregar código para detener cualquier proceso que ha
        }
    }
}
