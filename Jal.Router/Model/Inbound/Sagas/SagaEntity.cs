using System;

namespace Jal.Router.Model.Inbound.Sagas
{
    public class SagaEntity
    {
        public string Key { get; set; }
        public string Data { get; set; }
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