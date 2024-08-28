
using Avalonia.TodoList.WebApi.Data;
using Avalonia.TodoList.WebApi.Data.Dtos;
using Avalonia.TodoList.WebApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avalonia.TodoList.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        #region FIELDS
        private readonly TodoDbContext _context;
        private readonly ILogger<TodoController> _logger;
        #endregion

        #region CONSTRUCTOR
        public TodoController(ILogger<TodoController> logger, TodoDbContext context)
        {
            _logger = logger;
            _context = context;

            msg("Testing the logger.");
        }
        #endregion

        #region CONTROLLER METHODS
        [HttpGet("all", Name = "getalltodos")]
        public async Task<ActionResult<List<Todo>>> GetAllTodos()
        {
            try
            {
                var allTodos = await _context.Todos.ToListAsync<Todo>();
                return Ok(allTodos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodos()");
                return StatusCode(500, "GetAllUsers() : Internal server error");
            }
        }
        [HttpGet("{id}", Name = "gettodo")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            try
            {
                var todo = await _context.Todos.FirstOrDefaultAsync<Todo>(td => td.Id == id);

                if (todo == null)
                {
                    return NotFound($"Todo with ID {id} was not found.");
                }

                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetTodo)}()");
                return StatusCode(500, "GetAllUsers() : Internal server error");
            }
        }
        [HttpPost("create", Name = "createtodo")]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] CreateTodoDto newTodo)
        {

            Todo oTodo = new Todo()
            {
                Name = newTodo.Name,
                IsCompleted = newTodo.IsCompleted ?? false, // if in case IsCompleted not send in the webservice it will default to false.
                CreatedOn = DateTime.Now,
                CreatedBy = newTodo.CreatedBy ?? "Sample User"
            };

            await _context.Todos.AddAsync(oTodo);
            int rowsEffected = await _context.SaveChangesAsync();
            if (rowsEffected > 0)
            {
                return CreatedAtAction(nameof(GetTodo), new { id = oTodo.Id }, oTodo);
            }
            else
            {
                _logger.LogError($"{nameof(GetTodo)} : No rows were affected during the creation of the Todo item.");
                return StatusCode(500, "An error occurred while creating the Todo item.");
            }
        }
        [HttpPut("update/{id}", Name = "updatetodo")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, [FromBody] UpdateTodoDto updatedTodo)
        {
            try
            {
                // if the id and the id send thru the schema failed to match.
                if (id != updatedTodo.Id)
                {
                    return NotFound($"Id mismatched!, Todo with ID {id} and the Id send thru the schema({updatedTodo.Id}) do not match");
                }


                // Find the existing Todo item by ID
                var existingTodo = await _context.Todos.FirstOrDefaultAsync(td => td.Id == id);

                if (existingTodo == null)
                {
                    return NotFound($"Todo with ID {id} was not found.");
                }

                // Update the properties of the existing Todo
                existingTodo.Name = updatedTodo.Name;
                existingTodo.IsCompleted = updatedTodo.IsCompleted ?? existingTodo.IsCompleted; // Assuming you want to update this

                // Save the changes to the database
                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok(existingTodo); // Return the updated Todo
                }
                else
                {
                    _logger.LogError($"{nameof(UpdateTodo)}: No rows were affected during the update of the Todo item with ID {id}.");
                    return StatusCode(500, "An error occurred while updating the Todo item.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(UpdateTodo)}()");
                return StatusCode(500, "An internal server error occurred while updating the Todo item.");
            }
        }
        [HttpDelete("delete/{id}", Name = "deletetodo")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            try
            {
                // Find the existing Todo item by ID
                var existingTodo = await _context.Todos.FirstOrDefaultAsync(td => td.Id == id);

                if (existingTodo == null)
                {
                    return NotFound($"Todo with ID {id} was not found.");
                }

                // Remove the Todo item from the database
                _context.Todos.Remove(existingTodo);
                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return NoContent(); // Return 204 No Content to indicate successful deletion
                }
                else
                {
                    _logger.LogError($"{nameof(DeleteTodo)}: No rows were affected during the deletion of the Todo item with ID {id}.");
                    return StatusCode(500, "An error occurred while deleting the Todo item.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteTodo)}()");
                return StatusCode(500, "An internal server error occurred while deleting the Todo item.");
            }
        }
        #endregion

        // other methods
        private void msg(string sMessage)
        {
            _logger.LogInformation(sMessage);
        }
    }
}
