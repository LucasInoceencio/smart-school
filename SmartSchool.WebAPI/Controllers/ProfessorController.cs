using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repository;

        public ProfessorController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var professores = _repository.GetAllProfessores(true);
            return Ok(professores);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var professor = _repository.GetProfessorById(id, false);
            if (professor == null)
                return BadRequest("Professor não foi encontrado!");

            return Ok(professor);
        }

        // [HttpGet("byName")]
        // public IActionResult GetByName(string nome)
        // {
        //     var professor = _context.Professores.FirstOrDefault(fd =>
        //             fd.Nome.Contains(nome)
        //         );

        //     if (professor == null)
        //         return BadRequest("Professor não foi encontrado!");

        //     return Ok(professor);
        // }

        [HttpPost]
        public IActionResult Post(Professor professor)
        {
            #region Formatoa antigo usando apenas o context
            // _context.Add(professor);
            // _context.SaveChanges();
            // return Ok(professor);
            #endregion

            _repository.Add(professor);
            if (_repository.SaveChanges())
                return Ok(professor);

            return BadRequest("Professor não cadastrado!");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Professor professor)
        {
            var professorPersistido = _repository.GetProfessorById(id, false);
            if (professorPersistido == null)
                return BadRequest("Professor não encontrado!");

            _repository.Update(professor);
            if (_repository.SaveChanges())
                return Ok(professor);

            return BadRequest("Professor não atualizado!");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var professorPersistido = _repository.GetProfessorById(id);
            if (professorPersistido == null)
                return BadRequest("Professor não encontrado!");

            _repository.Update(professor);
            if (_repository.SaveChanges())
                return Ok(professor);

            return BadRequest("Professor não atualizado!");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var professor = _repository.GetProfessorById(id);
            if (professor == null)
                return BadRequest("Professor não encontrado!");

            _repository.Delete(professor);
            if (_repository.SaveChanges())
                return Ok("Professor deletado com sucesso!");

            return BadRequest("Professor não deletado!");
        }
    }
}