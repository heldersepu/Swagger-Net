using Swagger.Net.Annotations;
using Swagger.Net.Dummy.SwaggerExtensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    [SwaggerResponse(400, "Bad request")]
    public class SwaggerAnnotatedController : ApiController
    {
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(int))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid message", typeof(HttpError))]
        public int Create(Message message)
        {
            throw new NotImplementedException();
        }

        [SwaggerResponse( 200, Type = typeof( IEnumerable<Message> ), TypeName = "Messages" )]
        public IEnumerable<Message> GetAll()
        {
            throw new NotImplementedException();
        }

        [SwaggerOperationFilter(typeof(AddGetMessageExamples))]
        public Message GetById(int id)
        {
            throw new NotImplementedException();
        }

        [SwaggerResponse(200, MediaType = "image/png")]
        public HttpResponseMessage Patch()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [SwaggerOperation("UpdateMessage", Tags = new[] { "messages" }, Schemes = new[] { "ws" })]
        public void Put(int id, Message message)
        {
            throw new NotImplementedException();
        }
    }

    [SwaggerSchemaFilter(typeof(AddMessageDefault))]
    public class Message
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}