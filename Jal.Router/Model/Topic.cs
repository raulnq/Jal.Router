using System;

namespace Jal.Router.Model
{
    public class Topic
    {
        public Topic(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }
    }
}