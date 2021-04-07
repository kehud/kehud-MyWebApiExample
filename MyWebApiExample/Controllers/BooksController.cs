using MyWebApiExample.Data;
using MyWebApiExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;

namespace MyWebApiExample.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BooksController : ApiController
    {
        private readonly BooksDbContext _booksDbContext;

        public BooksController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BooksDB"];

            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString.ConnectionString);

            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            DbConnection dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;

            _booksDbContext = new BooksDbContext(dbConnection);
        }

        public BooksController(BooksDbContext booksDbContext)
        {
            if (booksDbContext == null)
                throw new ArgumentNullException("booksDbContext is null");

            _booksDbContext = booksDbContext;
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _booksDbContext.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }



        // GET: api/Books
        [HttpGet]
        [Route("api/GetBooks")]
        public IHttpActionResult GetBooks()
        {
            var booksRecrods = _booksDbContext.GetBooks();

            if (booksRecrods == null)
                return NotFound();
            else
                return Ok(booksRecrods);
        }

        // GET: api/Books/5
        [HttpGet]
        [Route("api/GetBook/{id}")]
        public IHttpActionResult GetBook([FromUri]int id)
        {
            var booksRecrod = _booksDbContext.GetBook(id);

            if (booksRecrod == null)
                return NotFound();
            else
                return Ok(booksRecrod);
        }

        // POST: api/Books
        [HttpPost]
        [Route("api/PostBook")]
        public IHttpActionResult PostBook([FromBody]BookModel bookModel)
        {
            try
            {
                _booksDbContext.BooksList.Add(bookModel);
                _booksDbContext.SaveChanges();

                return Ok(_booksDbContext.BooksList);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        // PUT: api/Books/5
        [HttpPut]
        [Route("api/PutBook")]
        public IHttpActionResult PutBook([FromBody]BookModel bookModel)
        {
            try
            {
                var booksRecrod = _booksDbContext.GetBook(bookModel.ID);

                booksRecrod.Name = bookModel.Name;
                booksRecrod.Category = bookModel.Category;
                booksRecrod.Price = bookModel.Price;

                _booksDbContext.SaveChanges();

                return Ok(_booksDbContext.BooksList);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        // DELETE: api/Books/5
        [HttpGet]
        [Route("api/DeleteBook/{id}")]
        public IHttpActionResult DeleteBook([FromUri]int id)
        {
            try
            {
                _booksDbContext.RemoveRecord(id);

                _booksDbContext.SaveChanges();

                return Ok(_booksDbContext.BooksList);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

    }
}
