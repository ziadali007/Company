using AutoMapper;
using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Dtos;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitRepository , IMapper mapper)
        {
            _unitOfWork = unitRepository;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string? Search)
        {
            IEnumerable<Department> departments;
            if (Search is not null)
            {
                departments =await _unitOfWork.departmentRepository.GetByNameAsync(Search);
                return View(departments);
            }
            else
            {
                departments =await _unitOfWork.departmentRepository.GetAllAsync();
                return View(departments);
            }
            //var departments = _departmentRepository.GetAll();
            //return View(departments);
        }

        public async Task<IActionResult> Search(string? Search)
        {
            IEnumerable<Department> departments;
            if (Search is not null)
            {
                departments = await _unitOfWork.departmentRepository.GetByNameAsync(Search);
                return PartialView("PartialViews/DepartmentTablePartialView", departments);
            }
            else
            {
                departments = await _unitOfWork.departmentRepository.GetAllAsync();
                return PartialView("PartialViews/DepartmentTablePartialView", departments);
            }
            //var departments = _departmentRepository.GetAll();
            //return View(departments);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto departmentDto)
        {
            if(ModelState.IsValid)
            {
                //var department = new Department()
                //{
                //    Code = departmentDto.Code,
                //    Name = departmentDto.Name,
                //    CreateAt = departmentDto.CreateAt
                //};
                var department = _mapper.Map<Department>(departmentDto);
                await _unitOfWork.departmentRepository.AddAsync(department);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(departmentDto);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var department =await _unitOfWork.departmentRepository.GetAsync(id.Value);

            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} Is Not Found" });

            return View(ViewName,department);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department =await _unitOfWork.departmentRepository.GetAsync(id.Value);

            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} Is Not Found" });

            //var createDepartmentDto = new CreateDepartmentDto()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //};
            var createDepartmentDto = _mapper.Map<CreateDepartmentDto>(department);
            return View(createDepartmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.departmentRepository.Update(department);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, CreateDepartmentDto department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var departmentModel = new Department()
        //        {
        //            Id = id,
        //            Code = department.Code,
        //            Name = department.Name,
        //            CreateAt = department.CreateAt
        //        };
        //        var Count = _departmentRepository.Update(departmentModel);
        //        if (Count > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(department);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} Is Not Found" });

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.departmentRepository.Delete(department);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }
    }
}
