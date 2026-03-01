using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Bodinis.WebApi.Controllers
{
   
        [ApiController]
        [Route("api/productos")]
        [Authorize]
    public class ProductosController: ControllerBase
    {
        private readonly ICUAdd<ProductoDtoAlta> _addProducto;
        private readonly ICUUpdate<ProductoDtoModificar> _updateProducto;
        private readonly ICUDeactivate _desactivarProducto;
        private readonly ICUGetAll<ProductoDtoListado> _getAllProductos;
        private readonly ICUGetById<ProductoDtoListado> _getProductoById;

        public ProductosController(
            ICUAdd<ProductoDtoAlta> addProducto,
            ICUUpdate<ProductoDtoModificar> updateProducto,
            ICUDeactivate desactivarProducto,
            ICUGetAll<ProductoDtoListado> getAllProductos,
            ICUGetById<ProductoDtoListado> getProductoById)
        {
            _addProducto = addProducto;
            _updateProducto = updateProducto;
            _desactivarProducto = desactivarProducto;
            _getAllProductos = getAllProductos;
            _getProductoById = getProductoById;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var productos = _getAllProductos.GetAll();
                return Ok(productos);
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
                var producto = _getProductoById.Execute(id);
                return Ok(producto);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (DatosInvalidosException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] ProductoDtoAlta dto)
        {
            try
            {
                _addProducto.Execute(dto);
                return StatusCode(201);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ProductoDtoModificar dto)
        {
            try
            {
                _updateProducto.Execute(id, dto);
                return NoContent();
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/desactivar")]
        public IActionResult Desactivar(int id)
        {
            try
            {
                _desactivarProducto.Execute(id);
                return NoContent();
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}

    


