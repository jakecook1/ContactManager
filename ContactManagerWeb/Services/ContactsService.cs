using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContactManagerWeb.Data;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Helpers;
using ContactManagerWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ContactManagerWeb.Services
{
    public class ContactsService : IService<Contact>
    {
        #region Init

        private readonly IUnitOfWork<DataContext> _uow;

        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private string UserName => _httpContextAccessor.HttpContext.User.Identity.Name;

        #endregion

        #region Public Methods

        public ContactsService(IUnitOfWork<DataContext> uow,
                               UserManager<User> userManager,
                               IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Contact> GetAll()
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            var entities = _uow.GetRepository<Contact>()
                           .GetAll(predicate: FilterByUserId(user.Id),
                                   include: Includes(),
                                   orderBy: OrderBy())
                           .ToList();

            var cols = new string[] {"UpdatedAt"};
            entities.UtcToLocalDates<Contact>(cols);

            return entities;
        }

        public async Task<IPaginate<Contact>> GetAllAsync(string sort, string search, int pageNumber)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            var entities = await _uow.GetRepositoryAsync<Contact>()
                                     .GetListAsync(predicate: GetFilter(search, user.Id),
                                                   include: Includes(),
                                                   orderBy: ListExtensions.GetOrderBy<Contact>(sort, "FirstName"),
                                                   index: pageNumber,
                                                   size: 10);

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
            var user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
                entity.User = user;

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

        public int GetActiveCount()
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            var entities = _uow.GetRepository<Contact>()
                               .GetAll(predicate: FilterActiveByUserId(user.Id),
                                   include: Includes())
                           .ToList();

            return entities.Count();
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

            if (contact.Email != entity.Email)
                contact.Email = entity.Email;
            
            if (contact.HomePhone != entity.HomePhone)
                contact.HomePhone = entity.HomePhone;

            if (contact.CellPhone != entity.CellPhone)
                contact.CellPhone = entity.CellPhone;

            if (contact.OfficeExtension != entity.OfficeExtension)
                contact.OfficeExtension = entity.OfficeExtension;

            if (contact.IrdNumber != entity.IrdNumber)
                contact.IrdNumber = entity.IrdNumber;

            if (contact.ImagePublicId != entity.ImagePublicId)
                contact.ImagePublicId = entity.ImagePublicId;

            if (contact.Active != entity.Active)
                contact.Active = entity.Active;
        }

        private static Expression<Func<Contact, bool>> GetFilter(string search, string userId)
        {
            Expression<Func<Contact, bool>> predicate = null;

            if (!string.IsNullOrEmpty(search))
            {
                predicate = source => source.User.Id == userId
                                      & (source.FirstName.ToLower() + " " + source.LastName.ToLower()).Contains(search.ToLower());
            }
            else
            {
                predicate = source => source.User.Id == userId;
            }

            return predicate;
        }

        private Expression<Func<Contact, bool>> FilterById(int id) => source => source.ContactId == id;

        private Expression<Func<Contact, bool>> FilterByUserId(string userId)
        {
            return source => source.User.Id == userId;
        }

        private Expression<Func<Contact, bool>> FilterActiveByUserId(string userId)
        {
            return source => source.Active & source.User.Id == userId;
        }

        private Func<IQueryable<Contact>, IIncludableQueryable<Contact, object>> Includes()
        {
            return source => source.Include(x => x.User);
        }

        private static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> OrderBy()
        {
            return source => source.OrderByDescending(x => x.UpdatedAt);
        }

        #endregion
    }
}