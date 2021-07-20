using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.V2.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.V2.Controllers
{
    /// <summary>
    /// Classe aluno controller testando
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public AlunoController(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        /// <summary>
        /// Será que vai funcionar essa bagaça
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var alunos = _repository.GetAllAlunos(true);

            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }

        // api/aluno
        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            #region Formato antigo usando apenas o context
            // _context.Add(aluno);
            // _context.SaveChanges();
            // return Ok(aluno);
            #endregion

            var aluno = _mapper.Map<Aluno>(model);
            _repository.Add(aluno);
            if (_repository.SaveChanges())
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));

            return BadRequest("Aluno não cadastrado!");
        }

        // api/aluno
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            var aluno = _repository.GetAlunoById(id);
            if (aluno == null)
                return BadRequest("Aluno não encontrado!");

            #region Formato utilizando apenas o context
            // _context.Update(aluno);
            // _context.SaveChanges();
            // return Ok(aluno);
            #endregion

            _mapper.Map(model, aluno);

            _repository.Update(aluno);
            if (_repository.SaveChanges())
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));

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

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}