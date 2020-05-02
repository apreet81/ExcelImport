using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExcelImport.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultDbConnection")
        {
        }
        public virtual DbSet<ImportedData> ImportedDatas { get; set; }
    }
}