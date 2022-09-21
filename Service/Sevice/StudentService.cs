using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Sevice
{
    public class StudentService: IStudentService
    {
        private readonly StudentManagementContext _studentManagementContext;

        public StudentService(StudentManagementContext studentManagementContext)
        {
            _studentManagementContext = studentManagementContext;
        }
        /// <summary>
        /// Delete All Student
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAllStudentAsync()
        {
            var temp = await _studentManagementContext.Students.ToListAsync();
            _studentManagementContext.RemoveRange(temp);
            await _studentManagementContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete Student By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyResponseObject<Student>> DeleteStudentAsync(int? id)
        {
            
                var entity = await _studentManagementContext.Students.FindAsync(id);

                if (entity == null)
                {
                    return new MyResponseObject<Student>
                    {
                        Data = null,
                        Message = "ok",
                        Code = 404,
                    };
                }
                _studentManagementContext.Remove(entity);
                await _studentManagementContext.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Data = entity,
                    Message = "ok",
                    Code = 200,
                };
        }

        /// <summary>
        /// Get All Student With Paginate
        /// </summary>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<MyResponseList<Student>> GetAllStudentAsync(int page = 1, int take = 10)
        {
            int count = await _studentManagementContext.Students.CountAsync();

                int totalPage = (int)Math.Ceiling(((decimal)count / take));
                int nextPage = 2;
                int curentPage = page;
                var data = await _studentManagementContext.Students.Skip((page - 1) * take).Take(take).ToListAsync();
                MyResponseList<Student> res = new MyResponseList<Student>()
                {
                    Message = "ok",
                    Code = 200,
                    Data = data,
                    Paginate = new Paginate()
                    {
                        TotalPage = totalPage,
                        CurentPage = curentPage,
                        NextPage = curentPage < totalPage ? curentPage + 1 : null,
                        PrePage = curentPage > 1 ? curentPage - 1 : null,
                        IsNextPage = curentPage < totalPage,
                        IsPrePage = curentPage > 1,
                    }
                };
                return res;
        }

        /// <summary>
        /// Get Student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyResponseObject<Student>> GetStudentAsync(int? id)
        {
             var entity = await _studentManagementContext.Students.FindAsync(id);
                if (entity == null)
                    return new MyResponseObject<Student>
                    {
                        Message = "Not Found",
                        Code = 404,
                        Data = null
                    };
                else
                    return new MyResponseObject<Student>
                    {
                        Message = "Ok",
                        Code = 200,
                        Data = entity
                    };
        }

        /// <summary>
        /// Insert Student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<MyResponseObject<Student>> InsertStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("entity");
            }

          
                await _studentManagementContext.Students.AddAsync(student);
                await _studentManagementContext.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Message = "ok",
                    Code = 200,
                    Data = student
                };
        }

        /// <summary>
        /// Search Student List
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MyResponseList<Student>> SearchStudentAsync(string? name)
        {
           
                var result = await _studentManagementContext.Students
                .Where(t => t.FirstName.Contains(name) || t.LastName.Contains(name)).ToListAsync();
                return new MyResponseList<Student>
                {
                    Message = "ok",
                    Data = result,
                    Code = 200
                };
        }

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<MyResponseObject<Student>> UpdateStudentAsync(Student student)
        {
            if (student == null)
            {
                return new MyResponseObject<Student>
                {
                    Message = "Not Found",
                    Code = 404,
                    Data = null
                };
            }


           
                var result = await _studentManagementContext.Students.FindAsync(student.Id);
                if(result == null)
                    return new MyResponseObject<Student>
                    {
                        Message = "Not Found",
                        Code = 404,
                        Data = null
                    };
                result.FirstName = student.FirstName;
                result.LastName = student.LastName;
                result.Email = student.Email;
                await _studentManagementContext.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Message = "ok",
                    Code = 200,
                    Data = result
                };
            }
    }
}
