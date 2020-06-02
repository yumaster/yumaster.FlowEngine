using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FlowEngine.Request
{
    public class QueryFlowInstanceListReq : PageReq
    {
        public string type { get; set; }
    }
}
