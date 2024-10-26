using System.ComponentModel.DataAnnotations;

namespace Demo.Dashboard.Models
{
	public class RoleFormViewModel
	{
		[Required(ErrorMessage ="Name is required")]
		[StringLength(256)]
        public string Name { get; set; }
    }
}
