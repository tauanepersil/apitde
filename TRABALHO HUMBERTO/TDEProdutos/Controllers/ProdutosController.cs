using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.context;
using TDEProdutos.Models;
using TDEProdutos.Validations;

namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProdutosController : Controller
    {
        private readonly ProdutoContext context;

        public static IWebHostEnvironment _environment;
        public ProdutosController(IWebHostEnvironment environment)

        {
            context = new ProdutoContext();
            _environment = environment;
        }

        ///<summary>
        /// Busca dados de um produto a partir do codigo
        /// Requer uso de token.
        /// </summary>
        /// <param name="codigo">codigo Produto</param>
        /// <returns>Objeto contendo os dados de um produto.</returns>

        [Authorize]
        [HttpGet("BuscarPorCodigo/{codigo}")]

        public ActionResult BuscarPorCodigo(string codigo)
        {
            return Ok(context._produtos.Find<Produto>(p => p.codigo == codigo).FirstOrDefault());

            
        }

        ///<summary>
        /// Adiciona dados de um produto no banco de dados
        /// Requer uso de token.
        /// </summary>
        /// <param name="produto">codigo Produto</param>
        /// <returns>Objeto contendo os dados de um produto.</returns>

        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult Adicionar(Produto produto)
        {
            ProdutoValidation validator = new ProdutoValidation();

            ValidationResult results = validator.Validate(produto);

            ProdutoValidation produtoValidation = new ProdutoValidation();
            var validacao = produtoValidation.Validate(produto);
            if (!validacao.IsValid)
            {
                List<string> erros = new List<string>();
                foreach(var failure in validacao.Errors)
                {
                    erros.Add("Property" + failure.PropertyName +
                        "failed validation. Error Was: "
                        + failure.ErrorMessage);
                }
                var resultado = context._produtos.Find<Produto>(p => p.codigo == produto.codigo).FirstOrDefault();
                if (resultado != null)
                {
                    return BadRequest("O produto existe na base de dados");
                }

               

                context._produtos.InsertOne(produto);

            }

          
            return Ok("produto cadastrado");

        }

        [HttpPost("upload")]
        public async Task<ActionResult> EnviaArquivo([FromForm] IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                if (arquivo.ContentType != "image/jpeg" &&
                    arquivo.ContentType != "image/jpg" &&
                    arquivo.ContentType != "image/png"
                   )
                {
                    return BadRequest("Formato Inválido de imagens");
                }

                try
                {

                    string contentRootPath = _environment.ContentRootPath;
                    string path = "";
                    path = Path.Combine(contentRootPath, "imagens");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream filestream = System.IO.File.Create(path + arquivo.FileName))
                    {
                        await arquivo.CopyToAsync(filestream);
                        filestream.Flush();
                        return Ok("Imagem enviada com sucesso " + arquivo.FileName);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            else
            {
                return BadRequest("Ocorreu uma falha no envio do arquivo...");
            }
        }

        ///<summary>
        /// Atualizar dados de um produto a partir do codigo
        /// Requer uso de token.
        /// </summary>
        /// <param name="codigo">codigo Produto</param>
        /// <returns>Objeto contendo os dados de um produto.</returns>


        [HttpPut("Atualizar/{codigo}")]
        public ActionResult Atualizar(string codigo, [FromBody] Produto produto)
        {
            
            var pResultado = context._produtos.Find<Produto>(p => p.codigo == codigo).FirstOrDefault();
            if (pResultado == null) return
                    NotFound("Id não encontrado, atualizacao não realizada!");

            produto.codigo = codigo;
            context._produtos.ReplaceOne<Produto>(p => p.codigo == codigo, produto);

            return NoContent();

        }

        ///<summary>
        /// Remover dados de um produto a partir do codigo
        /// Requer uso de token.
        /// </summary>
        /// <param name="codigo">codigo Produto</param>
        /// <returns>Objeto contendo os dados de um produto.</returns>

        [HttpDelete("Remover/{codigo}")]
        public ActionResult Remova(string codigo)
        {
            var pResultado = context._produtos.Find<Produto>(p => p.codigo == codigo).FirstOrDefault();
            if (pResultado == null) return
                    NotFound("Id não encontrado, atualizacao não realizada!");

            context._produtos.DeleteOne<Produto>(filter => filter.codigo == codigo);
            return NoContent();
        }

    }



    /* [HttpPut("desativar{Codigo}")]

     public ActionResult Desativar(string codigo)
     {
         var produtoDesativado = ListaProdutos.Where(P => P.codigo == codigo).FirstOrDefault();
         if (produtoDesativado == null) return NotFound("Produto não pode ser desativado, pois codigo não existe");
         if (produtoDesativado != null && produtoDesativado.Ativo == false) return BadRequest("Produto já está desativado, operação não realizada");
         produtoDesativado.Ativo = false;

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


             mail.Subject = "Produto desativado";
             mail.Body = "Olá, atenção. O produto " + codigo + " foi desativado do seu catalogo!";
         }

             return Ok("Prduto desativo");

     }*/




}

