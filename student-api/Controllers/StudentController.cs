using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Models;
using Service.Sevice;

namespace student_api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: api/Student
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetStudents(int page = 1)
        {
            var result = await _studentService.GetAllStudentAsync(page);
            return StatusCode(result.Code, result);
        }

        // GET: api/Student/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentService.GetStudentAsync(id);
            return StatusCode(student.Code, student);
        }


        // PUT: api/Student/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            student.Id = id;
            var result = await _studentService.UpdateStudentAsync(student);

            return StatusCode(result.Code, result);
        }

        // POST: api/Student
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostStudent(Student student)
        {
            var result = await _studentService.InsertStudentAsync(student);
            return StatusCode(result.Code, result);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<MyResponseObject<Student>> DeleteStudent(int id)
        {
            return await _studentService.DeleteStudentAsync(id);
        }

        // DELETE: api/Student/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteAll()
        {
            return await _studentService.DeleteAllStudentAsync();
        }
        //// GET: api/Search?name="name"
        [Route("/api/Students/Search")]
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<MyResponseList<Student>> SearchStudents(string name)
        {
            return await _studentService.SearchStudentAsync(name);
        }
    }
}
