using System;

namespace Jal.Router.Model
{
    public class SagaEntity
    {
        public string Name { get; private set; }

        public Type DataType { get; private set; }

        public int Timeout { get; private set; }

        private SagaEntity()
        {

        }

        public SagaEntity(string name, Type datatype, int timeout)
        {
            Name = name;
            DataType = datatype;
            Timeout = timeout;
        }
    }
}
