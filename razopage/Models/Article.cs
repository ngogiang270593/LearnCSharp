using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class Article
    {
        [Key]
        public int ID { get; set; }
        
        [Display(Name="Tiêu đề")]
        public string Title { get; set; } = default!;

        [DataType(DataType.Date)]
        [Display(Name="Ngày đăng")]
        public DateTime PublishDate { get; set; }

        [Display(Name="Nội dung")]
        public string Content {set; get;} = default!;
    }
}