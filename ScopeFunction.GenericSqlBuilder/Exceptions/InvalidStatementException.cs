using System.Runtime.Serialization;

namespace ScopeFunction.GenericSqlBuilder.Exceptions
{
    [Serializable]
    public class InvalidStatementException : Exception
    {
        public InvalidStatementException(string? message = null) : base(message ?? "The statement being defined is not valid")
        {
        
        }

        protected InvalidStatementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}

