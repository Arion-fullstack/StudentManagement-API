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
        /// <summary>
        /// Delete All Student
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAllStudentAsync()
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                var temp = await db.Students.ToListAsync();
                db.RemoveRange(temp);
                await db.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// Delete Student By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyResponseObject<Student>> DeleteStudentAsync(int? id)
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                var entity = await db.Students.FindAsync(id);

                if (entity == null)
                {
                    return new MyResponseObject<Student>
                    {
                        Data = null,
                        Message = "ok",
                        Code = 404,
                    };
                }
                db.Remove(entity);
                await db.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Data = entity,
                    Message = "ok",
                    Code = 200,
                };
            }
        }

        /// <summary>
        /// Get All Student With Paginate
        /// </summary>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<MyResponseList<Student>> GetAllStudentAsync(int page = 1, int take = 10)
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                int count = await db.Students.CountAsync();

                int totalPage = (int)Math.Ceiling(((decimal)count / take));
                int nextPage = 2;
                int curentPage = page;
                var data = await db.Students.Skip((page - 1) * take).Take(take).ToListAsync();
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
        }

        /// <summary>
        /// Get Student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyResponseObject<Student>> GetStudentAsync(int? id)
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                var entity = await db.Students.FindAsync(id);
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

            using (StudentManagementContext db = new StudentManagementContext())
            {
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Message = "ok",
                    Code = 200,
                    Data = student
                };
            }
        }

        /// <summary>
        /// Search Student List
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MyResponseList<Student>> SearchStudentAsync(string? name)
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                var result = await db.Students
                .Where(t => t.FirstName.Contains(name) || t.LastName.Contains(name)).ToListAsync();
                return new MyResponseList<Student>
                {
                    Message = "ok",
                    Data = result,
                    Code = 200
                };
            }
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
                    Code = 400,
                    Data = null
                };
            }


            using (StudentManagementContext db = new StudentManagementContext())
            {
                var result = await db.Students.FindAsync(student.Id);
                result.FirstName = student.FirstName;
                result.LastName = student.LastName;
                result.Email = student.Email;
                await db.SaveChangesAsync();

                return new MyResponseObject<Student>
                {
                    Message = "ok",
                    Code = 200,
                    Data = result
                };
            }
        }
    }
}
