using System;

namespace XamFormsTestApp.Data.Messages
{
    public class ClearOrderMessage
    {
        public static readonly string MessageId = Guid.NewGuid().ToString();
    }
}
