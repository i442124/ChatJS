using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatJS.Models.Messages
{
    public class ForumComponentModel
    {
        public byte[] Attachment { get; set; }

        [Required]
        [StringLength(255)]
        public string Content { get; set; }
    }
}
