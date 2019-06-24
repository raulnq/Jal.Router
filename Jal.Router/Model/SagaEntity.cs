using System;

namespace Jal.Router.Model
{
    public class SagaEntity
    {
        public string Id { get; set; }
        public object Data { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Ended { get; set; }
        public int? Timeout { get; set; }
        public string Status { get; set; }
        public double Duration { get; set; }
    }
}