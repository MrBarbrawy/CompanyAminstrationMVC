using AutoMapper;
using CompanyAdminstrationMVC.BLL;
using CompanyAdminstrationMVC.BLL.Interfaces;
using CompanyAdminstrationMVC.BLL.Repositories;
using CompanyAdminstrationMVC.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAdminstrationMVC.PL.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

     
        private readonly IMapper _mapper;


        public DepartmentController(
            
            IUnitOfWork unitOfWork,
            IMapper mapper
            ) 
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.AddAsync(model);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction("Index");

                }

            }

            return View(model);
        }
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();

            }
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (department is null)
            {
                return NotFound();
            }
            return View(viewName,department);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {    
            return await Details(id,"Update");

        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, Department department)
        {

            try
            {

                if (id != department.id) return BadRequest();
                 _unitOfWork.DepartmentRepository.Update(department);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return  RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty , ex.Message);
            }
            return View(department);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id,"Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id,Department department)
        {
            try
            {
                if (id != department.id) return BadRequest();
                 _unitOfWork.DepartmentRepository.Delete(department);
                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);
        }

    }
}
