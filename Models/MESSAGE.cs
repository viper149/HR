using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class MESSAGE
    {
        public int Id { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime SendAt { get; set; }
        [Required]
        public string SenderId { get; set; }

        public ApplicationUser Receiver { get; set; }
    }
}