using System;

namespace Neodenit.DialogAssistant.Shared.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public double CreditUsed { get; set; }

        public DateTime LastRequestDate { get; set; }
    }
}
