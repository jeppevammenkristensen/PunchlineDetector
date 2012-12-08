using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PunclineDetector.SimpleApi.Controllers
{
    public class SomeOtherController : ApiController
    {
        // GET api/someother
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/someother/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/someother
        public void Post([FromBody]string value)
        {
        }

        // PUT api/someother/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/someother/5
        public void Delete(int id)
        {
        }
    }
}
