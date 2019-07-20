using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jal.Router.Newtonsoft.Model
{
    public class NewtonsoftSerializerParameter
    {
        public NullValueHandling NullValueHandling { get; set; }

        public DefaultValueHandling DefaultValueHandling { get; set; }

        public Formatting Formatting { get; set; }

        public NewtonsoftSerializerParameter()
        {
            NullValueHandling = NullValueHandling.Ignore;
            DefaultValueHandling = DefaultValueHandling.Ignore;
            Formatting = Formatting.None;
        }
    }
}
