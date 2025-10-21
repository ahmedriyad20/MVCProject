
using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;

namespace FayoumDemo.Controllers
{
    public class ServiceController : Controller
    {
        private IDepartmentService _departmentServiceFromCTOR;
        private IDepartmentService _departmentServiceFromAction;

        public ServiceController(IDepartmentService departmentService)
        {
            _departmentServiceFromCTOR = departmentService;
        }

        public IActionResult Index([FromServices] IDepartmentService dept)
        {
            _departmentServiceFromAction = dept;

            ViewBag.deptFromCTOR = _departmentServiceFromCTOR;
            ViewBag.deptFromAction = _departmentServiceFromAction;
            return View();
        }
    }
}
