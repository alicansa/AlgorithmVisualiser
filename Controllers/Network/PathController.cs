using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DijkstraApi.Models;
using MongoDB.Bson;


namespace DijkstraApi.Controllers{

    public class PathController : Controller {

        private readonly IMapRepository _maps;

        public PathController(IMapRepository maps){
            this._maps = maps;
        }

        [HttpGet("maps/{mapId:length(24)}/shortestPaths")]
        public IActionResult Get(string mapId){
            if (!ModelState.IsValid){
                return StatusCode(400);
            }else{
                //return all the shortest paths
                var item = _maps.GetById(mapId);
                if (item == null) return NotFound();
                else return new ObjectResult(item.ShortestPaths);
            }
        }

        [HttpGet("maps/{mapId:length(24)}/shortestPaths/{nodeId}")]
        public IActionResult Get(string mapId,string nodeId){
            if (!ModelState.IsValid){
                return StatusCode(400);
            }else{
                //return the shortest path
                var item = _maps.GetById(mapId);
                if (item == null) return NotFound();
                else return new ObjectResult(item.ShortestPaths[nodeId]);
            }
        }



        [HttpGet("maps/{mapId:length(24)}/shortestPaths/{nodeId}/")]
        public IActionResult Get(string mapId,string nodeId,[FromQuery]INode itinerary){
            if (!ModelState.IsValid){
                return StatusCode(400);
            }else{
                //return the shortest path
                var item = _maps.GetById(mapId);
                if (item == null) return NotFound();
                else return new ObjectResult(item.ShortestPaths[nodeId][itinerary.Id]);
            }
        }

        public class Itinerary{
            public string StartNode {get;set;}
            public string EndNode {get;set;}
        }
    }

}