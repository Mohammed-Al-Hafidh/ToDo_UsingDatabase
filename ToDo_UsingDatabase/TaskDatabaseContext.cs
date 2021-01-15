using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_UsingDatabase
{
    class TaskDatabaseContext : DbContext
    {
        const string databasename = "ToDoDatabase.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, databasename);
        public TaskDatabaseContext() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30")
        {

        }
        public DbSet<Task> Tasks { get; set; }
    }
}
