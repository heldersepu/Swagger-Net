using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class MetadataAnnotatedTypesController : ApiController
    {
        public int Create(PaymentWithMetadata payment)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));

            throw new NotImplementedException();
        }

        public string Get([RegularExpression("^[3-6]?\\d{12,15}$")]string CardNum)
        {
            return CardNum;
        }
    }

    public class PaymentMetadata
    {
        [Required]
        public decimal Amount { get; set; }

        [Required, RegularExpression("^[3-6]?\\d{12,15}$")]
        public string CardNumber { get; set; }

        [Required, Range(1, 12)]
        public int ExpMonth { get; set; }

        [Required, Range(14, 99)]
        public int ExpYear { get; set; }

        [StringLength(500, MinimumLength = 10)]
        public string Note { get; set; }
    }

    [MetadataType(typeof(PaymentMetadata))]
    public class PaymentWithMetadata {
        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public string Note { get; set; }
    }
}