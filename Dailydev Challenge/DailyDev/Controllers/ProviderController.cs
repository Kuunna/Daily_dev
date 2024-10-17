using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly ProviderRepository _providerRepository;

        public ProviderController(ProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<Provider> GetById(int id)
        {
            var provider = _providerRepository.GetById(id);
            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Provider provider)
        {
            _providerRepository.Add(provider);
            return CreatedAtAction(nameof(GetById), new { id = provider.Id }, provider);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Provider provider)
        {
            provider.Id = id;
            _providerRepository.Update(provider);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _providerRepository.Delete(id);
            return NoContent();
        }
    }

}
