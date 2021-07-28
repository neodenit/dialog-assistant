using System.Collections.Generic;

namespace Neodenit.DialogAssistant.Shared.Models
{
    public class Dialog
    {
        public int ID { get; set; }

        public ICollection<Message> Messages { get; set; }

        public User User1 { get; set; }

        public User User2 { get; set; }
    }
}
