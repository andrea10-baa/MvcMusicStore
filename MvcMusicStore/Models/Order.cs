using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [DisplayName("Apellido")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "El departamento/estado es obligatorio")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [DisplayName("Código Postal")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [DisplayName("Correo Electrónico")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "El correo no es válido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}