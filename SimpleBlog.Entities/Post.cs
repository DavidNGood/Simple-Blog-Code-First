using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SimpleBlog.Entities
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }

        [ForeignKey("ApplicationUser")]
        [Required]
        public string UserID { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string PostBody { get; set; }

        public string ImageName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Category Category { get; set; }
    }
}
