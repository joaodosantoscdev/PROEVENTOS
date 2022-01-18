using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve ter entre, 4 e 60 caractéres")]
        public string Local { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DateEvent { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve ter entre, 4 e 60 caractéres")]
        public string Theme { get; set; }

        [ Required(ErrorMessage = "O campo {0} é obrigatório"),
        Display(Name="capacidade"),
        Range(20, 120000, ErrorMessage = "{0} não pode ser menor que 20, e maior que 120.000")]
        public int Capacity { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida. (gif, jpg, jpeg, bmp ou png)")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
         Display(Name = "telefone"),
         Phone(ErrorMessage = "O campo {0} está com formato inválido")]
        public string CallNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
        Display(Name = "e-mail"),
        EmailAddress(ErrorMessage = "É necessário ser um {0} válido")]
        public string Email { get; set; }

        public IEnumerable<PartDTO> Parts { get; set; }
        public IEnumerable<SocialMediaDTO> SocialMedias { get; set; }
        public IEnumerable<SpeakerDTO> Speaker { get; set; }
    }
}
