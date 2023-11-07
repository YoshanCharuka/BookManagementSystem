    using BookMgtSystem.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace BookMgtSystem.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class BookController : ControllerBase
        {
            private readonly BookAPIDBContext _dbContext;

            public BookController(BookAPIDBContext dbContext)
            {
                _dbContext = dbContext;
            }
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
            {
                if (_dbContext == null)
                {
                    return NotFound();
                }

                return await _dbContext.Books.ToListAsync();
            }
            [HttpGet("{id}")]
            public async Task<ActionResult<Book>> GetBook(int id)
            {
                if (_dbContext.Books == null)
                {
                    return NotFound();
                }
                var book = await _dbContext.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return book;

            }
            [HttpPost]
            public async Task<ActionResult<Book>> AddBook(Book book)
            {
                _dbContext.Books.Add(book);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            [HttpPut]
            public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
            {
                if (id != book.Id)
                {
                    return BadRequest();
                }
                _dbContext.Entry(book).State = EntityState.Modified;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookAvailable(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;

                    }
                }
                return Ok();
            }
            private bool BookAvailable(int id)
            {
                return (_dbContext.Books?.Any(x => x.Id == id)).GetValueOrDefault();
            }
            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteBook(int id)
            {
                if(_dbContext.Books == null)
                {
                    return NotFound();
                }
                var book = await _dbContext.Books.FindAsync(id);
                if(book == null)
                {
                    return NotFound();  
                }
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

        }
    }
