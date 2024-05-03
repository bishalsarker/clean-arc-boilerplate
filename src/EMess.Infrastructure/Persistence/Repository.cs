using EMess.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Npgsql;

namespace EMess.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> table;
        public IDbConnection Connection { 
            get { 
                return new SqlConnection(_context.Database.GetConnectionString());
            } 
        }

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            table = _context.Set<T>();
        } 

        public async Task<IEnumerable<T>> GetMultiple(string query, object param)
        {
            using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
            {   
                return await connection.QueryAsync<T>(query, param);
            }
        }

        public async Task<T> GetSingle(string query, object param)
        {
            using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
            {
                return (await connection.QueryFirstOrDefaultAsync<T>(query, param)) ?? default!;
            }
        }

        public async Task InsertAsync(T obj)
        {
            await table.AddAsync(obj);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = table.Find(id) ?? default!;
            if (existing != null)
            {
                table.Remove(existing);
            }
        }
        public async Task<bool> SaveAsync()
        {
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0 ? true : false;
        }
    }
}
