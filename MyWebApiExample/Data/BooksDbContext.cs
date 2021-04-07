using MyWebApiExample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWebApiExample.Data
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbConnection dbConnection) : base(dbConnection, true){}

        #region tbl prop

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //map table
            modelBuilder.Entity<BookModel>().ToTable("BooksList");
            //map key
            modelBuilder.Entity<BookModel>().HasKey(k => k.ID);
            //Identity
            modelBuilder.Entity<BookModel>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public DbSet<BookModel> BooksList { get; set; }

        #endregion


        #region qry

        //get books
        public IQueryable<BookModel> GetBooks()
        {
            return BooksList;
        }

        //get book by ID
        public BookModel GetBook(int id)
        {
            return BooksList.Where(a => a.ID == id).First();
        }

        public void RemoveRecord(int id)
        {
            BooksList.RemoveRange(BooksList.Where(b => b.ID == id));
        }
        
        #endregion

    }
}