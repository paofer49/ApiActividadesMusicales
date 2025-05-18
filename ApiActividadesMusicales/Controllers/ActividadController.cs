using ApiActividadesMusicales.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiActividadesMusicales.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ActividadController : ControllerBase
    {
        private readonly string _connectionString;

        public ActividadController(IConfiguration configuration) 
        { 
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Actividad>> GetActs()
        {
            var acts = new List<Actividad>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ActividadesMusicales", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    acts.Add(new Actividad
                    {
                        IdActividad = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Fecha = reader.GetDateTime(3),
                        TipoActividadId = reader.GetInt32(4),
                        Lugar = reader.GetString(5)
                    });
                }
            }
            return Ok(acts);
        }

        [HttpPost]
        public IActionResult CreateAct([FromBody] Actividad act) 
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("InsertarActividad", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", act.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", act.Descripcion);
                    cmd.Parameters.AddWithValue("@Fecha", act.Fecha);
                    cmd.Parameters.AddWithValue("@TipoActividadId", act.TipoActividadId);
                    cmd.Parameters.AddWithValue("@Lugar", act.Lugar);

                    cmd.ExecuteNonQuery();
                    return Ok("Actividad creada correctamente");
                }
            }
            catch (SqlException ex)
            {
                return BadRequest($"Error al crear la actividad: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAct(int id, [FromBody] Actividad act) 
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ActualizarActividad", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdActividad", id);
                    cmd.Parameters.AddWithValue("@Nombre", act.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", act.Descripcion);
                    cmd.Parameters.AddWithValue("@Fecha", act.Fecha);
                    cmd.Parameters.AddWithValue("@TipoActividadId", act.TipoActividadId);
                    cmd.Parameters.AddWithValue("@Lugar", act.Lugar);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0) return NotFound();
                }
                return Ok("Actividad actualizada correctamente.");
            }
            catch (SqlException ex)
            {

                return BadRequest($"Error al actualizar la actividad: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAct(int id) 
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString)) 
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EliminarActividad", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdActividad", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0) return NotFound();
                }
                return Ok("Actividad eliminada correctamente.");
            }
            catch (SqlException ex)
            {

                return BadRequest($"Error al actualizar la actividad: {ex.Message}");
            }
        }
    }
}
