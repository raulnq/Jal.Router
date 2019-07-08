using System;

namespace Jal.Router.Model
{
    public class SagaEntity
    {
        public string Name { get; }

        public Type DataType { get; }

        public SagaData SagaData { get; }

        public SagaEntity()
        {

        }

        public SagaEntity(string name, Type datatype)
        {
            Name = name;
            DataType = datatype;
        }
    }
}
