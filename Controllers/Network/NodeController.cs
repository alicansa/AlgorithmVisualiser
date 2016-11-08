using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DijkstraApi.Models;
using MongoDB.Bson;


namespace DijkstraApi.Controllers{

  
    public class NodeController : Controller {

        private readonly IMapRepository _maps;

        public NodeController(IMapRepository maps){
            this._maps = maps;
        }

        //create new node
        [HttpPost("maps/{mapId:length(24)}/nodes/")]
        public void Post(string mapId,[FromBody]Node node){
            if (!ModelState.IsValid){
                HttpContext.Response.StatusCode = 400;
            }else{
                
                var map = _maps.GetById(mapId);

                if(map == null) HttpContext.Response.StatusCode = 404;
                else{
                    map.AddNode(node);
                    _maps.Update(map);
                }
            
                // var url = Url.RouteUrl("GetMapById", new {id = map.Id}, Request.Scheme,
                //     Request.Host.ToUriComponent());
                // HttpContext.Response.StatusCode = 201;
                // HttpContext.Response.Headers["Location"] = url;
            }
        }

        [HttpPost("maps/{mapId:length(24)}/nodes/{nodeId}")]
        public void Post(string mapId,string nodeId,[FromQuery]NodeNeighbor neighbor){
            if (!ModelState.IsValid){
                HttpContext.Response.StatusCode = 400;
            }else{
                
                var map = _maps.GetById(mapId);

                if(map == null) HttpContext.Response.StatusCode = 404;
                else{
                    if (map.AddConnection(nodeId,neighbor.Id,neighbor.Distance))
                        _maps.Update(map);
                    else HttpContext.Response.StatusCode = 404;
                    
                }
            
                // var url = Url.RouteUrl("GetMapById", new {id = map.Id}, Request.Scheme,
                //     Request.Host.ToUriComponent());
                // HttpContext.Response.StatusCode = 201;
                // HttpContext.Response.Headers["Location"] = url;
            }
        }

        // update node
        // [Route("maps/{mapId:length(24)}/nodes/{nodeId:string}")]
        // [HttpPut] 
        // public void Put(string id,[FromBody]Node node){
        //     if (!ModelState.IsValid){
        //         HttpContext.Response.StatusCode = 400;
        //     }else{
        //         _maps.Update(map);
        //     }
        // }

        // delete node
        [HttpDelete("maps/{mapId:length(24)}/nodes/{nodeId}")] 
        public IActionResult Delete(string mapId,string nodeId){
            var map = _maps.GetById(mapId);

            if (map == null) return NotFound();
            else {
                if(map.RemoveNode(nodeId)) {
                    _maps.Update(map);
                    return StatusCode(204);
                }else return NotFound();
            }

        }

    }

}