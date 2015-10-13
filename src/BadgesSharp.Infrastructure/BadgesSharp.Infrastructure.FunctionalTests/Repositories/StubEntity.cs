using System;
using Skahal.Infrastructure.Framework.Domain;

namespace BadgesSharp.Infrastructure.FunctionalTests
{
    public class StubEntity : EntityWithIdBase<string>, IAggregateRoot
    {
        public StubEntity()
        {
            Id = string.Empty;
        }

        public string Text1 { get; set; }

        public string Text2 { get; set; }

        public string Text3 { get; set; }

        public DateTime Date { get; set; }

        public int Integer { get; set; }

        public string ReadOnly
        {
            get
            {
                return "Read only property";
            }
        }
    }
}
