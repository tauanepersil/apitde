using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDEProdutos.Models
{
    public class Produto
    {
        [BsonRepresentation(BsonType.ObjectId)]
    

        public string codigo { get; set; }

        public string NomeProduto { get; set; }

        public string descricaoProduto { get; set; }
        public float precoVendas { get; set; }
        public float precoCusto { get; set; }

        public DateTime dataCadastro { get; set; }

        public float estoque { get; set; }

        public string imagem { get; set; }

        public float Altura { get; set; }

        public float Largura { get; set; }

        public float Profundidade { get; set; }

        public string categoriaProduto { get; set; }

        public bool Ativo{ get; set; }

        public int IDcategoria { get; set; }

        public Categoria Categoria { get; set; }

        public int EstoqueAtual { get; set; }

        public int EstoqueMinimo { get; set; }
    }
}
