using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIwebCore.Model;

namespace APIwebCore
{
    [Route("api/TachesDTO")]
    [ApiController]
    public class TodoItemsControllerDTO : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsControllerDTO(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TachesDTO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems.Select(s=> ItemToDTO(s)).ToListAsync();
        }

        // GET: api/TachesDTO/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }
           // la transformation ne se fait que ici
            return ItemToDTO(todoItem);
        }

        // PUT: api/TachesDTO/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            // le client ne connait que le DTO (les champs id, name, isComplete)
            // Le champs secret est inconnu

            // test si l'id envoyé au put == id contenu dans le json envoyé par le client
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }
            // ceci n est plus valable ----> _context.Entry(todoItemDTO).State = EntityState.Modified;

            // on récupère un TodoItemDTO qui vient du mapping du json envoyé par le client sur la classe modèle
            // dans la BDD on a des objets TodoItem et non DTO qui est un masque d'affichage

            // on récupère l'enregistrement dans la BDD
            var todoItem = await _context.TodoItems.FindAsync(id);
            if(todoItem == null)
            {
                return NotFound();
            }
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            // C'est là qu on peut faire l'enregistrement des changements apportés
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TachesDTO
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            TodoItem todoItem = new TodoItem
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };

            // soit on traite le secret ici par la couche métier
            // soit c'est un autre code qui le traite

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }

        // DELETE: api/TachesDTO/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDTO>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return ItemToDTO(todoItem);
            // return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
        
        // Créer une nouvelle instance de TodoItem
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };


        // Euuivalent du code ci dessous
        //private static TodoItemDTO ItemToDTO2(TodoItem todoItem)
        //{
        //    TodoItemDTO to = new TodoItemDTO();
        //    to.Id = todoItem.Id;
        //    to.Name = todoItem.Name;
        //    to.IsComplete = todoItem.IsComplete;
        //    return to;
        //}
    }
}
