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

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ContactViewModel() { Active = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Contact>(viewModel);
                await _contactsService.AddAsync(entity);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = _contactsService.Get(id.Value);
            var viewModel = _mapper.Map<ContactViewModel>(entity);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Contact>(viewModel);

                _contactsService.Update(entity);

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = _contactsService.Get(id.Value);

            if (entity == null)
                return NotFound();

            return View(_mapper.Map<ContactViewModel>(entity));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _contactsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
