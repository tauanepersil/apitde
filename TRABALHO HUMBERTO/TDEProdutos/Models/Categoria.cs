using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDEProdutos.Models
{
    public class Categoria
    {
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        public int IDcategoria { get; set; }

        public string descricao { get; set; }
    }
}
