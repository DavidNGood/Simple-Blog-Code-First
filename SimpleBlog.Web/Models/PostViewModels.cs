using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SimpleBlog.Web.Models
{
    public class CreatePostViewModel
    {
        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Please enter a proper date.")]
        [Display(Name = "Post Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Post")]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string PostBody { get; set; }

        [DataType(DataType.Upload)]
        public string ImageName { get; set; }
    }

    public class EditPostViewModel
    {
        public int PostID { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [DataType(DataType.Date, ErrorMessage="Please enter a proper date.")]
        [Display(Name = "Post Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.Html)]
        [Display(Name = "Post")]
        [AllowHtml]
        public string PostBody { get; set; }

        public string ImageName { get; set; }

        public EditPostViewModel() { }
    }
}