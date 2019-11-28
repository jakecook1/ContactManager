using System.Threading.Tasks;
using AutoMapper;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Models;
using ContactManagerWeb.Services;
using ContactManagerWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactManagerWeb.Controllers
{
    public class ContactsController : BaseController
    {
        #region Init Methods

        private readonly ILogger<HomeController> _logger;

        private readonly IMapper _mapper;

        private readonly IService<Contact> _contactsService;

        public ContactsController(ILogger<HomeController> logger,
                                  IMapper mapper,
                                  IService<Contact> contactsService,
                                  UserManager<User> userManager) : base(userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _contactsService = contactsService;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Index(string sort, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sort;
            ViewData["FirstNameSortParam"] = string.IsNullOrEmpty(sort) ? "FirstName_desc" : "";
            ViewData["LastNameSortParam"] = sort == "LastName" ? "LastName_desc" : "LastName";

            ViewData["CurrentFilter"] = searchString;

            var result = await _contactsService.GetAllAsync(sort, searchString, pageNumber ?? 0);

            var viewModels = _mapper.Map<Paginate<ContactViewModel>>(result);

            return View(viewModels);
        }

        #endregion
    }
}
