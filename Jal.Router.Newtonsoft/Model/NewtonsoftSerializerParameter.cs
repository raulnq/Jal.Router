using Newtonsoft.Json;

namespace Jal.Router.Newtonsoft
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
