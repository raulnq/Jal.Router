using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class SagaInfo
    {
        public string Id { get; set; }

        public List<string> ParentIds { get; set; }

        public SagaInfo()
        {
            ParentIds = new List<string>();
        }
    }
}