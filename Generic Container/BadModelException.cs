namespace Generic_Container
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BadModelException : Exception
    {
        public BadModelException() { }

        public BadModelException(string message)
            : base(message) { }

        public BadModelException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BadModelException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}