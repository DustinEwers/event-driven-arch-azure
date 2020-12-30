using System;

namespace GameBot.Domain
{
    public class OrderResult
    {
        public Guid OrderId { get; set; }
        public bool OrderSuccessful { get; set; }
        public string User { get; set; }
    }
}
