using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quanyi.Entity.HttpModel
{
    /// <summary>
    /// SAP接口专用
    /// </summary>
    public class HttpResponseModal<T> where T : class
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<T> Items { get; set; }
    }
}
