using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Web.Models
{
    public class CreateCategoryViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public EditCategoryViewModel() { }
    }
}