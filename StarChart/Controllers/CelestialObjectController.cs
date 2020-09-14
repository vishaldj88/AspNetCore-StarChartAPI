using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // GET: api/<CelestialObjectController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CelestialObjectController>/5
        [HttpGet("{id:int}")]
        [Route("GetById")]
        public IActionResult GetById(int id)
        {

            var result = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            

            if (result == null)
            {
                return NotFound();
            }
            else 
            {
                foreach (var sat in _context.CelestialObjects)
                {
                    if (sat.OrbitedObjectId == result.Id)
                    {
                        result.Satellites = new List<CelestialObject> { sat};
                    }

                }
            }

            return Ok(result);
        }

        [HttpGet("{name}")]     
        [Route("name")]
        public IActionResult GetByName(string name)     
        {

            var result =  _context.CelestialObjects.Where(x => x.Name == name) as IEnumerable<CelestialObject>;


            foreach (var sat in _context.CelestialObjects)
            {

                foreach (var res in result)
                {
                    if (sat.OrbitedObjectId == res.Id)
                    {
                        res.Satellites = new List<CelestialObject> { sat };
                    }
                }
            }


            if (result.Count()==0 ||result == null)
            {
                return NotFound();
            }
            

            return Ok(result);
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {

            var result = _context.CelestialObjects;

           
                foreach (var sat in _context.CelestialObjects)
                {

                    foreach (var res in result )
                    {
                        if (sat.OrbitedObjectId == res.Id)
                        {
                            res.Satellites = new List<CelestialObject> { sat };
                        }
                    }
                }

                if (result.Count() == 0 || result == null)
                {
                    return NotFound();
                }
         

            return Ok(result);
        }

        // POST api/<CelestialObjectController>
        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {

            _context.Add(celestialObject);
            _context.SaveChanges();

           

            // overload with three arguments
            return CreatedAtRoute(
                routeName: "GetById",
                routeValues: new { id = celestialObject.Id },
                value:  celestialObject );
        }

        

        // DELETE api/<CelestialObjectController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (result!=null)
            {
                _context.CelestialObjects.RemoveRange(result);
                _context.SaveChanges();
                
            }

            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }


            if (result != null)
            {
                result.Name =name;
                _context.SaveChanges();
            }

            return NoContent(); 
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var result = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }


            if (result != null)
            {
                result.Name = celestialObject.Name;
                result.OrbitedObjectId = celestialObject.OrbitedObjectId;
                result.OrbitalPeriod = celestialObject.OrbitalPeriod;
                _context.SaveChanges();
            }

            return NoContent();
        }
    }
}
