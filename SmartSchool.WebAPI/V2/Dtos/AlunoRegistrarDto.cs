using System;

namespace SmartSchool.WebAPI.V2.Dtos
{
    public class AlunoRegistrarDto
    {
        /// <summary>
        /// O identificador do Aluno no banco de dados.
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataInicio { get; set; } = DateTime.Now;
        public DateTime? DataFinal { get; set; } = null;
        public bool Ativo { get; set; } = true;
    }
}