using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glab.Domains.Models.Faculties;

namespace Glab.Infrastructures.Storages.FacultiesStorages
{
    public interface IFacultyStorage
    {
        Task<List<Faculty>> SelectFaculties();


    }
}
