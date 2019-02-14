using System;
using System.Runtime.Serialization;

namespace TodoApp.Contracts.Services
{
    [Serializable]
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message)
        {
        }

        protected ItemNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}