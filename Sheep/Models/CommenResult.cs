using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sheep.Models
{
    public class CommenResult : IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;
        HttpStatusCode _statusCode;
        string _mediaType = "application/json";

        public CommenResult(object value, HttpRequestMessage request, HttpStatusCode statusCode = HttpStatusCode.OK)
        {

            _value = JsonHelper.Serialize(value);
            _request = request;
            _statusCode = statusCode;
        }
        public CommenResult(object value, HttpRequestMessage request, string mediaType, HttpStatusCode statusCode = HttpStatusCode.OK)
        {

            _value = JsonHelper.Serialize(value);
            _request = request;
            _statusCode = statusCode;
            _mediaType = mediaType;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = _statusCode,
                Content = new StringContent(_value, Encoding.GetEncoding("UTF-8"), _mediaType),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }
    }
}