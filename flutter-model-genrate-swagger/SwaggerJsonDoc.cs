using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flutter_model_genrate_swagger
{

    public class SwaggerJsonDoc
    {
        public Components components { get; set; }
    }
    public class Components
    {
        public Dictionary<string, object> schemas { get; set; }
    }
}
