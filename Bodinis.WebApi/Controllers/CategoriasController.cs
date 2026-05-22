using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICUAdd<CategoriaDtoAlta> _addCategoria;
        private readonly ICUGetAll<CategoriaDtoListado> _getAllCategorias;
        private readonly ICUGetById<CategoriaDtoListado> _getCategoriaById;
        private readonly ICUUpdate<CategoriaDtoModificar> _updateCategoria;
        private readonly ICUDelete<CategoriaDtoListado> _deleteCategoria;

        public CategoriasController(
            ICUAdd<CategoriaDtoAlta> addCategoria,
            ICUGetAll<CategoriaDtoListado> getAllCategorias,
            ICUGetById<CategoriaDtoListado> getCategoriaById,
            ICUUpdate<CategoriaDtoModificar> updateCategoria,
            ICUDelete<CategoriaDtoListado> deleteCategoria)
        {
            _addCategoria = addCategoria;
            _getAllCategorias = getAllCategorias;
            _getCategoriaById = getCategoriaById;
            _updateCategoria = updateCategoria;
            _deleteCategoria = deleteCategoria;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_getAllCategorias.Execute());
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_getCategoriaById.Execute(id));
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] CategoriaDtoAlta dto)
        {
            try
            {
                _addCategoria.Execute(dto);
                return StatusCode(201);
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CategoriaDtoModificar dto)
        {
            try
            {
                _updateCategoria.Execute(id, dto);
                return NoContent();
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteCategoria.Execute(id);
                return NoContent();
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
