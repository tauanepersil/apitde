using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.Models;

namespace TDEProdutos.context
{
    public class ProdutoContext
    {
       
            public MongoDatabase Database;
        public String DataBaseName = "test";

        string conexaoMongoDB = "mongodb+srv://teste:teste@cluster0.zjwp5.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
        public IMongoCollection<Produto> _produtos;
        public IMongoCollection<Categoria> _categoria;
        public IMongoCollection<Usuario> _usuarios;


        //public MongoCollection Pessoas { get; set; }

        public ProdutoContext()
        {
            var cliente = new MongoClient(conexaoMongoDB);
            var server = cliente.GetDatabase(DataBaseName);
            _produtos = server.GetCollection<Produto>("Produtos");
            _categoria = server.GetCollection<Categoria>("Categoria");
            _usuarios = server.GetCollection<Usuario>("Usuario");

        }
    }
}
