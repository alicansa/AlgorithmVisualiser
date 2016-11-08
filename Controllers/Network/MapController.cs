using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DijkstraApi.Models;
using MongoDB.Bson;


namespace DijkstraApi.Controllers{

    
    public class MapController : Controller {

        private readonly IMapRepository _maps;

        public MapController(IMapRepository maps){
            this._maps = maps;
        }

        // Get a list of maps 
        [HttpGet("maps")]
        public IEnumerable<MapSummary> Get(){
            var maps = _maps.GetMapList();

            return maps;
        }

        // Get all the details of a specific map 
        [HttpGet("maps/{id:length(24)}")]
        public IActionResult Get(string id){

            var item = _maps.GetById(id);

            if (item == null) return NotFound();
            else return new ObjectResult(item);
        }

        // create a new map 
        [HttpPost("maps")]
        public void Post([FromBody]MapSummary map){
            if (!ModelState.IsValid){
                HttpContext.Response.StatusCode = 400;
            }else{
                
                _maps.Add(new Map(map.Name));
            
                // var url = Url.RouteUrl("GetMapById", new {id = map.Id}, Request.Scheme,
                //     Request.Host.ToUriComponent());
                // HttpContext.Response.StatusCode = 201;
                // HttpContext.Response.Headers["Location"] = url;
            }
        }

        // update a specific map
        [HttpPut("maps/{id:length(24)}")]
        public void Put(string id,[FromBody]Map map){
            if (!ModelState.IsValid){
                HttpContext.Response.StatusCode = 400;
            }else{
                _maps.Update(map);
            }
        }

        // Delete a specific map
        [HttpDelete("maps/{id:length(24)}")]
        public IActionResult Delete(string id){
            if (_maps.Remove(id)){
                return StatusCode(204);
            }
            return NotFound();
        }
    }

}