using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.V1.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.V1.Controllers
{
    /// <summary>
    /// Classe aluno controller testando
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
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

        // Route
        // api/aluno/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repository.GetAlunoById(id, false);

            if (aluno == null)
                return BadRequest("Aluno não foi encontrado!");

            return Ok(_mapper.Map<AlunoDto>(aluno));
        }

        // Query String
        // api/aluno/byId?id=1
        [HttpGet("byId")]
        public IActionResult GetById2(int id)
        {
            var aluno = _repository.GetAlunoById(id, false);

            if (aluno == null)
                return BadRequest("Aluno não foi encontrado!");

            return Ok(_mapper.Map<AlunoDto>(aluno));
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
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoRegistrarDto model)
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