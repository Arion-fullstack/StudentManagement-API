using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IStudentService
    {
        Task<MyResponseList<Student>> GetAllStudentAsync(int page = 1, int take = 10);
        Task<MyResponseObject<Student>> GetStudentAsync(int? id);
        Task<MyResponseObject<Student>> InsertStudentAsync(Student student);
        Task<MyResponseObject<Student>> DeleteStudentAsync(int? id);
        Task<bool> DeleteAllStudentAsync();
        Task<MyResponseObject<Student>> UpdateStudentAsync(Student student);
        Task<MyResponseList<Student>> SearchStudentAsync(string? name);
    }
}
