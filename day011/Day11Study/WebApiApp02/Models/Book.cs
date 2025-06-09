using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace WebApiApp02.Models
{
    public class Book
    {
        // Key
        [Key]
        public int Idx { get; set; }
        // 책제목
        public string Names { get; set; }
        // 책저자
        public string Author { get; set; }
        // 출판일
        public DateOnly ReleaseDate { get; set; }
    }
}
