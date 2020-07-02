using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.HttpModel
{
    public class HttpResponseModel
    {
        public SuccessResponseModel SuccessReponse { get; set; }
        public ErrorResponseModel ErrorResponse { get; set; }

    }
}
