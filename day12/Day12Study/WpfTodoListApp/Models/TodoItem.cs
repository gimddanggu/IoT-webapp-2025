using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations;

namespace WpfTodoListApp.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required] // NotNulll 일 경우 string에 ?(Nullable)을 삭제할 것
        [Column(TypeName = "VARCHAR(100)")] // 이거 사용안 하면 테이블 컬럼이 LongText로 만들어짐
        public string? Title { get; set; }
        [Required]
        [Column(TypeName = "CHAR(8)")] //20250610
        public string TodoDate { get; set; }
        
        // boolean
        public bool IsComplete { get; set; }

    }
}



