using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ContactManagerWeb.Constants;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Enums;
using ContactManagerWeb.Helpers;
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
            // View data for sort and search values
            GetViewData(sort, searchString);

            // Get all contacts for specific user
            var entities = await _contactsService.GetAllAsync(sort, searchString, pageNumber ?? 0);

            // Check if max contacts has been created
            ViewBag.CanCreate = entities.Count < IntConstants.MaxContacts;

            // Map to view model
            var viewModels = _mapper.Map<Paginate<ContactViewModel>>(entities);

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var entities = _contactsService.GetAll();

            // If a user navigates to this route from address bar send them back to index if max contacts created
            if (entities.Count() >= IntConstants.MaxContacts)
                return RedirectToAction(nameof(Index), "Contacts");

            // Return create contact view
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

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = _contactsService.Get(id.Value);
            var viewModel = _mapper.Map<ContactViewModel>(entity);

            return View(viewModel);
        }

        public IActionResult ViewInfo(int? id)
        {
            if (id == null)
                return NotFound();

            var entity = _contactsService.Get(id.Value);
            var viewModel = _mapper.Map<ContactViewModel>(entity);

            return PartialView("~/Views/Shared/Modals/_ContactModal.cshtml", viewModel);
        }

        public IActionResult Export()
        {
            // Export data as a csv file
            var entities = _contactsService.GetAll();

            var streamFile = new FileBuilder(FileType.Csv).GetFile<Contact>(entities);
            return File(streamFile.Contents, streamFile.ContentType, streamFile.Name);
        }

        [HttpGet]
        public IActionResult Print()
        {
            // Get all contacts for logged in user
            var entities = _contactsService.GetAll();

            // Map to view model
            var viewModels = _mapper.Map<List<ContactViewModel>>(entities);

            // Return create contact view
            return View(viewModels);
        }

        #endregion

        #region Private Methods

        private void GetViewData(string sort, string searchString)
        {
            ViewData["CurrentSort"] = sort;
            ViewData["FirstNameSortParam"] = string.IsNullOrEmpty(sort) ? "FirstName_desc" : "";
            ViewData["EmailSortParam"] = sort == "Email" ? "Email_desc" : "Email";
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
                entity.ImagePublicId = null;

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
