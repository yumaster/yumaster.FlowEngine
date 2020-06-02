using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FlowEngine.Request
{
    public class QueryFlowInstanceHistoryReq : PageReq
    {
        /// <summary>
        /// 流程实体名称
        /// </summary>
        public string FlowInstanceId { get; set; }

    }
}
