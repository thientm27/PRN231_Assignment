using CarRenting.Repositories.Repo;
using CarRenting.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using CarRentingOData.Repositories;

namespace CarRentingOData.API.Controllers
{
    public class CarProducerController : ODataController
    {
        private readonly ICarProducerRepo _repository = new CarProducerRepo();

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetAsync();
            return Ok(result);
        }
    }
}
