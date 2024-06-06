using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service1.Model
{
    public class Model1
    {
        [Column("user_id")]
        public Guid Id { get; set; }
        public string Name {get;set; }
        public string? Description {get;set; }
    }
}
