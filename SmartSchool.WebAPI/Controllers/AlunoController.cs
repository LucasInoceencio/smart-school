using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IRepository _repository;

        public AlunoController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.GetAllAlunos(true);
            return Ok(result);
        }

        // Route
        // api/aluno/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repository.GetAlunoById(id, false);

            if (aluno == null)
                return BadRequest("Aluno não foi encontrado!");

            return Ok(aluno);
        }

        // Query String
        // api/aluno/byId?id=1
        [HttpGet("byId")]
        public IActionResult GetById2(int id)
        {
            var aluno = _repository.GetAlunoById(id, false);

            if (aluno == null)
                return BadRequest("Aluno não foi encontrado!");

            return Ok(aluno);
        }

        // api/aluno/nome
        // [HttpGet("{nome}")]
        // public IActionResult GetByName(string nome)
        // {
        //     var aluno = _context.Alunos.FirstOrDefault(fd =>
        //         fd.Nome.Contains(nome));

        //     if (aluno == null)
        //         return BadRequest("Aluno não foi encontrado!");

        //     return Ok(aluno);
        // }

        // api/aluno/byName?nome=Marta&sobrenome=Kent
        // [HttpGet("byName")]
        // public IActionResult GetByName(string nome, string sobrenome)
        // {
        //     var aluno = _context.Alunos.FirstOrDefault(fd =>
        //         fd.Nome.Contains(nome) && fd.Sobrenome.Contains(sobrenome));

        //     if (aluno == null)
        //         return BadRequest("Aluno não foi encontrado!");

        //     return Ok(aluno);
        // }

        // api/aluno
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            #region Formato antigo usando apenas o context
            // _context.Add(aluno);
            // _context.SaveChanges();
            // return Ok(aluno);
            #endregion

            _repository.Add(aluno);
            if (_repository.SaveChanges())
                return Ok(aluno);

            return BadRequest("Aluno não cadastrado!");
        }

        // api/aluno
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            var alunoPersistido = _repository.GetAlunoById(id);
            if (alunoPersistido == null)
                return BadRequest("Aluno não encontrado!");

            #region Formato utilizando apenas o context
            // _context.Update(aluno);
            // _context.SaveChanges();
            // return Ok(aluno);
            #endregion

            _repository.Update(aluno);
            if (_repository.SaveChanges())
                return Ok(aluno);

            return BadRequest("Aluno não atualizado!");
        }

        // api/aluno
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            var alunoPersistido = _repository.GetAlunoById(id);
            if (alunoPersistido == null)
                return BadRequest("Aluno não encontrado!");

            #region Formato utilizando apenas o context
            // _context.Update(aluno);
            // _context.SaveChanges();
            // return Ok(aluno);
            #endregion

            _repository.Update(aluno);
            if (_repository.SaveChanges())
                return Ok(aluno);

            return BadRequest("Aluno não atualizado!");
        }

        // api/aluno
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var aluno = _repository.GetAlunoById(id);

            if (aluno == null)
                return BadRequest("Aluno não encontrado!");

            #region Formato antigo usando apenas context
            // _context.Remove(aluno);
            // _context.SaveChanges();
            // return Ok();
            #endregion

            _repository.Delete(aluno);
            if (_repository.SaveChanges())
                return Ok("Aluno deletado!");

            return BadRequest("Aluno não deletado!");
        }
    }
}