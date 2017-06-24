using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XRMS.Business.Models;
using XRMS.Business.Services;
using XRMS.Libraries.BaseObjects;
using XRMS.WebApi.Models;

namespace XRMS.WebApi.Controllers
{
    public class AreaController : ApiController
    {

        AreaManager _manager = new AreaManager();
        // GET: api/Area
        public IEnumerable<AreaDto> Get()
        {
            List<AreaDto> list = new List<AreaDto>();
            foreach(Area item in _manager.GetList())
            {
                list.Add(new AreaDto
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Description = item.Description
                });
            }
            return list;
        }

        // GET: api/Area/5
        public AreaDto Get(int id)
        {
            Area item = _manager.GetById(id);
            AreaDto result = new AreaDto();
            result.Id = item.Id;
            result.Code = item.Code;
            result.Name = item.Name;
            result.Description = item.Description;
            return result;
        }

        // POST: api/Area
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Area/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Area/5
        public void Delete(int id)
        {
        }
    }
}
