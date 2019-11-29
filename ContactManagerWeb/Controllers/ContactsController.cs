using System.Text.RegularExpressions;
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
            GetViewData(sort, searchString);

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

                AddImage(viewModel, entity);

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

                AddImage(viewModel, entity);

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

        public IActionResult ViewInfo(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = _contactsService.Get(id.Value);
            var viewModel = _mapper.Map<ContactViewModel>(entity);

            return PartialView("~/Views/Shared/Modals/_ContactModal.cshtml", viewModel);
        }

        #endregion

        #region Private Methods

        private void GetViewData(string sort, string searchString)
        {
            ViewData["CurrentSort"] = sort;
            ViewData["FirstNameSortParam"] = string.IsNullOrEmpty(sort) ? "FirstName_desc" : "";
            ViewData["LastNameSortParam"] = sort == "LastName" ? "LastName_desc" : "LastName";
            ViewData["HomePhoneSortParam"] = sort == "HomePhone" ? "HomePhone_desc" : "HomePhone";
            ViewData["CellPhoneSortParam"] = sort == "CellPhone" ? "CellPhone_desc" : "CellPhone";
            ViewData["OfficeExtensionSortParam"] = sort == "OfficeExtension" ? "OfficeExtension_desc" : "OfficeExtension";
            ViewData["IrdNumberSortParam"] = sort == "IrdNumber" ? "IrdNumber_desc" : "IrdNumber";
            ViewData["ActiveSortParam"] = sort == "Active" ? "Active_desc" : "Active";

            ViewData["CurrentFilter"] = searchString;
        }

        private static void AddImage(ContactViewModel viewModel, Contact entity)
        {
            var deletedImage = viewModel.DeletedImage;

            if (deletedImage)
                entity.ImagePublicId  = null;

            var uploadDetails = viewModel.UploadDetails;

            if (!string.IsNullOrEmpty(uploadDetails))
            {
                var details = Regex.Split(uploadDetails, @"#\$%");
                entity.ImagePublicId = $"v{details[1]}/{details[0]}";
            }
        }

        #endregion
    }
}
