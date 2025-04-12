using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SIMS.ViewModels
{
    public class CreateStudentViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int MajorId { get; set; }
        public int ClassId { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Majors { get; set; }


        [BindNever]        
        public IEnumerable<SelectListItem> Classes { get; set; }

    }
}
    
