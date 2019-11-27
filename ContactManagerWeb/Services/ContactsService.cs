using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContactManagerWeb.Data;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Helpers;
using ContactManagerWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;

namespace ContactManagerWeb.Services
{
    public class ContactsService : IService<Contact>
    {
        #region Init

        private readonly IUnitOfWork<DataContext> _uow;

        private readonly UserManager<User> _userManager;

        public readonly IMemoryCache _cache;

        #endregion

        #region Public Methods

        public ContactsService(IUnitOfWork<DataContext> uow,
                               UserManager<User> userManager,
                               IMemoryCache cache)
        {
            _uow = uow;
            _userManager = userManager;
            _cache = cache;
        }

        public IEnumerable<Contact> GetAll(string userId = null)
        {
            var plansRepo = _uow.GetRepository<Contact>();
            var entities = new List<Contact>();

            if (!string.IsNullOrEmpty(userId))
            {
                entities = _uow.GetRepository<Contact>()
                               .GetAll(predicate: FilterByUserId(userId),
                                       orderBy: OrderBy())
                               .ToList();
            }
            else
            {
                entities = _uow.GetRepository<Contact>()
                               .GetAll(orderBy: OrderBy())
                               .ToList();
            }

            var cols = new string[] {"UpdatedAt"};
            entities.UtcToLocalDates<Contact>(cols);

            return entities;
        }

        public async Task<IPaginate<Contact>> GetAllAsync(string sort, string search, int pageNumber)
        {
            var entities = await _uow.GetRepositoryAsync<Contact>()
                                     .GetListAsync(predicate: GetFilter(search),
                                                   orderBy: ListExtensions.GetOrderBy<Contact>(sort, "FirstName"),
                                                   index: pageNumber,
                                                   size: 50);

            var cols = new string[] {"UpdatedAt"};
            entities.Items.UtcToLocalDates<Contact>(cols);

            return entities;
        }

        public Contact Get(object id)
        {
            var entity = _uow.GetRepository<Contact>()
                             .Single(predicate: FilterById((int)id));

            var cols = new string[] {"UpdatedAt"};
            entity.UtcToLocalDates<Contact>(cols);

            return entity;
        }

        public async Task AddAsync(Contact entity)
        {
            await _uow.GetRepositoryAsync<Contact>().AddAsync(entity);
            _uow.SaveChanges();
        }

        public void Update(Contact entity)
        {
            // Need to set to utc, need to figure out a way to do this in the model
            var cols = new string[] {};
            entity.LocalToUtcDates<Contact>(cols);

            var contact = _uow.GetRepository<Contact>()
                           .Single(predicate: x => x.ContactId == entity.ContactId,
                                   disableTracking: false);

            CheckColumnUpdates(entity, contact);

            _uow.GetRepository<Contact>().Update(contact);
            _uow.SaveChanges();
        }

        public void Delete(object id)
        {
            _uow.GetRepository<Contact>().Delete(id);
            _uow.SaveChanges();
        }

        #endregion

        #region Private Methods

        private void CheckColumnUpdates(Contact entity, Contact contact)
        {
            if (contact.FirstName != entity.FirstName)
                contact.FirstName = entity.FirstName;

            if (contact.MiddleInitial != entity.MiddleInitial)
                contact.MiddleInitial = entity.MiddleInitial;

            if (contact.LastName != entity.LastName)
                contact.LastName = entity.LastName;
            
            if (contact.HomePhone != entity.HomePhone)
                contact.HomePhone = entity.HomePhone;

            if (contact.CellPhone != entity.CellPhone)
                contact.CellPhone = entity.CellPhone;

            if (contact.OfficeExtension != entity.OfficeExtension)
                contact.OfficeExtension = entity.OfficeExtension;

            if (contact.IrdNumber != entity.IrdNumber)
                contact.IrdNumber = entity.IrdNumber;

            if (contact.Active != entity.Active)
                contact.Active = entity.Active;
        }


        private static Expression<Func<Contact, bool>> GetFilter(string search)
        {
            Expression<Func<Contact, bool>> predicate = null;

            if (!string.IsNullOrEmpty(search))
                predicate = source => source.FirstName.ToLower().Contains(search.ToLower());

            return predicate;
        }

        private Expression<Func<Contact, bool>> FilterById(int id) => source => source.ContactId == id;

        private Expression<Func<Contact, bool>> FilterByUserId(string userId)
        {
            return source => source.User.Id == userId;
        }

        private static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> OrderBy()
        {
            return source => source.OrderByDescending(x => x.UpdatedAt);
        }

        #endregion
    }
}