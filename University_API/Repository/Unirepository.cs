using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using University_API.Data;
using University_API.Models;
using University_API.Repository.IRepository;

namespace University_API.Repository
{
    public class Unirepository : IUniRepository
    {
        private readonly ApplicationDbContext _db;

        public Unirepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(University entity)
        {
            await _db.Universities.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(University entity)
        {
             _db.Universities.Update(entity);
            await SaveAsync();

        }
        public async Task<University> GetAsync(Expression<Func<University,bool>>? filter = null, bool tracked = true)
        {
            IQueryable<University> query = _db.Universities;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<List<University>> GetAllAsync(Expression<Func<University,bool>>? filter = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<University> query = _db.Universities.OrderByDescending(u=>u.IsBookmark); 


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageSize > 0)
            {
                if (pageSize > 50)
                {
                    pageSize = 50;
                }

                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            return await query.ToListAsync();
               
        }

        public async Task RemoveAsync(University entity)
        {
           // entity.LastModified = DateTime.Now;
            _db.Universities.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();
        }
    }
}
