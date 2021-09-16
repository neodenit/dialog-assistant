using System;

namespace Neodenit.DialogAssistant.Shared.Models
{
    public class Message
    {
        public int ID { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public DateTime SendTime { get; set; }

        public int DialogID { get; set; }
    }
}
