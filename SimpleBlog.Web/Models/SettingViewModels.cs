using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Web.Models
{
    public class CreateSettingViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }
    }

    public class EditSettingViewModel
    {
        public int SettingID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }

        public EditSettingViewModel() { }
    }
}