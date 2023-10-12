using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public Message() { }

        [Key]
        public long Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Text { get; set; }
    }
}