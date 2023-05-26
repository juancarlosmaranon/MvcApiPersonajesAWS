using Microsoft.AspNetCore.Mvc;
using MvcApiPersonajesAWS.Services;

namespace MvcApiPersonajesAWS.Controllers
{
    public class PersonajesController : Controller
    {
        private readonly ServiceApiPersonajes apiPersonajes;

        public PersonajesController(ServiceApiPersonajes apiPersonajes)
        {
            this.apiPersonajes = apiPersonajes;
        }

        public IActionResult ApiPersonajes()
        {
            return View();
        }

        public async Task<IActionResult> ApiEC2Personajes()
        {
            return View(await this.apiPersonajes.GetPersonajesEC2Async());
        }
    }
}
