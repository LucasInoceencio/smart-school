using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public ProfessorController(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var professores = _repository.GetAllProfessores(true);
            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(professores));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var professor = _repository.GetProfessorById(id, false);
            if (professor == null)
                return BadRequest("Professor não foi encontrado!");

            var professorDto = _mapper.Map<ProfessorDto>(professor);

            return Ok(professorDto);
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
        public IActionResult Post(ProfessorRegistrarDto model)
        {
            #region Formatoa antigo usando apenas o context
            // _context.Add(professor);
            // _context.SaveChanges();
            // return Ok(professor);
            #endregion

            var professor = _mapper.Map<Professor>(model);
            _repository.Add(professor);
            if (_repository.SaveChanges())
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));

            return BadRequest("Professor não cadastrado!");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ProfessorRegistrarDto model)
        {
            var professor = _repository.GetProfessorById(id, false);
            if (professor == null)
                return BadRequest("Professor não encontrado!");

            _mapper.Map(model, professor);

            _repository.Update(professor);
            if (_repository.SaveChanges())
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));

            return BadRequest("Professor não atualizado!");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistrarDto model)
        {
            var professor = _repository.GetProfessorById(id);
            if (professor == null)
                return BadRequest("Professor não encontrado!");

            _mapper.Map(model, professor);

            _repository.Update(professor);
            if (_repository.SaveChanges())
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));

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