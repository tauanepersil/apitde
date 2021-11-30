using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.context;
using TDEProdutos.Models;
using TDEProdutos.Validations;


namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ProdutoContext context;


        public CategoriaController()
        {
            context = new ProdutoContext();

        }

      

            [HttpGet("BuscarPorCategoria/{categoria}")]

            public ActionResult BuscarPorCategoria(string Categoria)
            {
            return Ok(context._produtos.Find<Produto>(c => c.codigo == Categoria).FirstOrDefault());
        }

        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult AdicionarCategoria (Categoria categoria)
        {
            CategoriaVallidation categoriavallidation = new CategoriaVallidation();
            var validacao = categoriavallidation.Validate(categoria);
            if (!validacao.IsValid)
            {
                List<string> erros = new List<string>();
                foreach (var failure in validacao.Errors)
                {
                    erros.Add("Property" + failure.PropertyName +
                        "failed validation. Error Was: "
                        + failure.ErrorMessage);
                }
            }
            return Ok();
        }



        /*

         [HttpPut("desativar{IDcategoria}")]

         public ActionResult Desativar(int IDcategoria)
         {
             var CATEGORIADesativado = ListaCategoria.Where(C => C.IDcategoria == IDcategoria).FirstOrDefault();
             if (CATEGORIADesativado == null) return NotFound("Produto não pode ser desativado, pois codigo não existe");


             using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
             {
                 smtp.Host = "smtp.gmail.com";
                 smtp.Port = 587;
                 smtp.EnableSsl = true;
                 smtp.UseDefaultCredentials = false;
                 smtp.Credentials = new System.Net.NetworkCredential("alanaraxa2019@gmail.com", "SUASENHAdoEmail");
             }

             using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
             {
                 mail.From = new System.Net.Mail.MailAddress("alanaraxa2019@gmail.com");
                 mail.To.Add(new System.Net.Mail.MailAddress("alanaraxa2019@gmail.com"));


                 mail.Subject = "IDcategoria desativado";
                 mail.Body = "Olá, atenção. O ID " + IDcategoria + " foi desativado do seu catalogo!";
             }

             return Ok("IDcategoria desativo");

         }*/

        [HttpPut("Atualizar/{id}")]
        public ActionResult Atualizar(string id, [FromBody] Categoria categoria)
        {

            var pResultado = context._categoria.Find<Categoria>(c => c.Id == id).FirstOrDefault();
            if (pResultado == null) return
                    NotFound("Id não encontrado, atualizacao não realizada!");

            categoria.Id = id;
            context._categoria.ReplaceOne<Categoria>(c => c.Id == id, categoria);

            return NoContent();

        }



        [HttpDelete("Remover/{id}")]
        public ActionResult Remova(string id)
        {
            var pResultado = context._categoria.Find<Categoria>(c => c.Id == id).FirstOrDefault();
            if (pResultado == null) return
                    NotFound("Id não encontrado, atualizacao não realizada!");

            context._categoria.DeleteOne<Categoria>(filter => filter.Id == id);
            return NoContent();
        }







    }
}
