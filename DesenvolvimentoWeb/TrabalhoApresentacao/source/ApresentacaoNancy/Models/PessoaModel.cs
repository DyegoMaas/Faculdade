using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dominio;

namespace ApresentacaoNancy.Models
{
    public class PessoaModel : IValidatableObject
    {
        public long? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo Nome � obrigat�rio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CPF � obrigat�rio.")]
        public long? Cpf { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ValidadorCpf.ValidarCpf(Cpf.Value.ToString()))
            {
                yield return new ValidationResult("O CPF informado n�o � v�lido.", new[]{"CPF"});
            }
        }
    }
}