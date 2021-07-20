using System;

namespace SmartSchool.WebAPI.V1.Dtos
{
    /// <summary>
    /// Classe utilizada para alterar aluno
    /// </summary>
    public class AlunoDto
    {
        /// <summary>
        /// Identificador da chave do Aluno no banco de dados
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public int Idade { get; set; }
        public DateTime DataInicio { get; set; }
        public bool Ativo { get; set; } = true;
    }
}