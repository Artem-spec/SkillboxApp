using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Infrastructure.Entity
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        public List<TodoRecord> TodoRecords { get; set; }
    }
}
