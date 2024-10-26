using CompanyAdminstrationMVC.BLL.Repositories;
using CompanyAdminstrationMVC.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using CompanyAdminstrationMVC.BLL.Interfaces;
using System.Collections.ObjectModel;
using AutoMapper;
using CompanyAdminstrationMVC.PL.ViewModels;
using System.Reflection.Metadata;
using CompanyAdminstrationMVC.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

    
namespace CompanyAdminstrationMVC.PL.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(

            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeViewModels = new Collection<EmployeeViewModel>();
            if (string.IsNullOrEmpty(searchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(searchInput);
            }

            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);



            string Message = "Hello World";


            return View(result);
        }

        [Authorize(Roles ="HR")]      
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information

            // View's Dictionary : 
            // 1.ViewData
            ViewData["Departments"] = departments;
            // 2.ViewBag
            // 3.TempData
            return View();
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    if (model.Image is not null) 
                    {
                        model.ImageName = FilesSettings.UploadFile(model.Image, "Images");
                    
                    }
                    var employee = _mapper.Map<Employee>(model);


                     await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    var Count = await _unitOfWork.CompleteAsync();

                    if (Count > 0)
                    {

                        TempData["Message"] = "Employee is Created Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Employee is Not Created Successfully";
                    }
                    return RedirectToAction(nameof(Index));



                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }


            return View(model);
        }

        [Authorize(Roles ="HR")]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();

            }
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound();
            }
            var result = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewName, result);
        }

        [Authorize(Roles ="Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information

            ViewData["Departments"] = departments;

            return await Details(id, "Update");

        }

        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
        {

            try
            {

                if (ModelState.IsValid) {

                    if(model.ImageName is not null)
                    { 
                    FilesSettings.DeleteFile(model.ImageName , "Images");
                    
                    
                    }

                    if (model.Image is not null)
                    {
                        model.ImageName = FilesSettings.UploadFile(model.Image, "Images");

                    }



                    var employee = _mapper.Map<Employee>(model);

                if (id != model.Id) return BadRequest();
                 _unitOfWork.EmployeeRepository.Update(employee);
                    var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
                
                
                
                
                }





            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "HR")]
        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();


                if (ModelState.IsValid) { 
                
               

                var employee = _mapper.Map<Employee>(model);

                 _unitOfWork.EmployeeRepository.Delete(employee);
                    var Count = await _unitOfWork.CompleteAsync();

                    if (Count > 0)
                {
                        if (model.ImageName is not null)
                        {
                            FilesSettings.DeleteFile(model.ImageName, "Images");


                        }
                        return RedirectToAction("Index");
                }
                
                
                
                

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }




    }
}
