using System.ComponentModel.DataAnnotations;

namespace WebApiApp03.Models
{
    public class IoT_Datas
    {
        [Key]
        public int Id { get; set; } 
        public DateTime Sensing_Dt { get; set; }   
        public string loc_Id { get; set; }  
        public float temp { get; set; }
        public float humid { get; set; }    
    }
}
