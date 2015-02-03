namespace Controllers
{
    public class MainController
    {
        private readonly IMainControllerService _service;

        public MainController(IMainControllerService service)
        {
            _service = service;
        }

        public void Method1()
        {
            Data =_service.Method1();
        }

        public void Method2()
        {
            Data = _service.Method2();
        }

        public string Data { get; set; }
    }

    public interface IMainControllerService
    {
        string Method1();
        string Method2();
    }
}
