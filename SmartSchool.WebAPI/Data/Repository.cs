using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public class Repository : IRepository
    {
        private readonly SmartContext _context;

        public Repository(SmartContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<PageList<Aluno>> GetAllAlunosAsync(
            PageParams pageParams,
            bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Disciplina)
                    .ThenInclude(ic => ic.Professor);
            }

            query = query.AsNoTracking().OrderBy(or => or.Id);

            if(!string.IsNullOrEmpty(pageParams.Nome))
                query = query.Where(wh => 
                    wh.Nome.ToUpper().Contains(pageParams.Nome.ToUpper())
                    || wh.Sobrenome.ToUpper().Contains(pageParams.Nome.ToUpper()));

            if(pageParams.Matricula > 0)
                query = query.Where(wh => wh.Matricula == pageParams.Matricula);

            if(pageParams.Ativo != null)
                query = query.Where(wh => wh.Ativo == pageParams.Ativo);

            // return await query.ToListAsync();
            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Disciplina)
                    .ThenInclude(ic => ic.Professor);
            }

            query = query.AsNoTracking().OrderBy(or => or.Id);
            return query.ToArray();
        }

        public Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Disciplina)
                    .ThenInclude(ic => ic.Professor);
            }

            query = query.AsNoTracking()
                .Where(wh =>
                    wh.AlunosDisciplinas.Any(an => an.DisciplinaId == disciplinaId))
                .OrderBy(or => or.Id);
            return query.ToArray();
        }

        public Professor[] GetAllProfessores(bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (includeAluno)
            {
                query = query.Include(ic => ic.Disciplinas)
                    .ThenInclude(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Aluno);
            }
            query = query.AsNoTracking().OrderBy(or => or.Id);
            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (includeAluno)
            {
                query = query.Include(ic => ic.Disciplinas)
                    .ThenInclude(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Aluno);
            }

            query = query.AsNoTracking()
                .OrderBy(or => or.Id)
                .Where(wh => wh.Disciplinas.Any(
                    an => an.AlunosDisciplinas.Any(an => an.DisciplinaId == disciplinaId)
                ));

            return query.ToArray();
        }

        public Aluno GetAlunoById(int alunoId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Disciplina)
                    .ThenInclude(ic => ic.Professor);
            }

            query = query.AsNoTracking().Where(wh => wh.Id == alunoId).OrderBy(or => or.Id);
            return query.FirstOrDefault();
        }

        public Professor GetProfessorById(int professorId, bool includeAluno = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (includeAluno)
            {
                query = query.Include(ic => ic.Disciplinas)
                    .ThenInclude(ic => ic.AlunosDisciplinas)
                    .ThenInclude(ic => ic.Aluno);
            }

            query = query.AsNoTracking()
                .OrderBy(or => or.Id)
                .Where(wh => wh.Id == professorId);

            return query.FirstOrDefault();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}