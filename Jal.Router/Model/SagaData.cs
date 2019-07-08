using System;

namespace Jal.Router.Model
{
    public class SagaData
    {
        public string Id { get; set; }
        public object Data { get; }
        public string DataType { get; }
        public string Name { get; }
        public DateTime Created { get; }
        public DateTime? Updated { get; private set; }
        public DateTime? Ended { get; private set; }
        public int? Timeout { get; }
        public string Status { get; private set; }
        public double Duration { get; private set; }

        public SagaData(string id, object data, string datetype, string name, DateTime created, int? timeout, string status, DateTime? updated, DateTime? ended, double duration)
            :this(data, datetype, name, created, timeout, status)
        {
            Id = id;
            Updated = created;
            Ended = ended;
            Duration = duration;
        }

        public SagaData(object data, string datetype, string name, DateTime created, int? timeout, string status)
        {
            Data = data;
            DataType = datetype;
            Name = name;
            Created = created;
            Timeout = timeout;
            Status = status;
            Updated = created;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void UpdateUpdatedDateTime(DateTime updated)
        {
            Updated = updated;
        }

        public void UpdateEndedDateTime(DateTime ended)
        {
            Ended = ended;
            Duration = (Ended.Value - Created).TotalMilliseconds;
        }
    }
}