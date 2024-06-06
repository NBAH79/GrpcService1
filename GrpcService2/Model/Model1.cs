using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service2.Model
{
    public class Model1
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name {get;set; } = "";
        public string? Description {get;set; }
    }
}
