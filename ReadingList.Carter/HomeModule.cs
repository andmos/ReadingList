using Carter;
using Microsoft.AspNetCore.Http;

namespace ReadingList.Carter
{
    public class HomeModule : CarterModule
    {
        public HomeModule()
        {
            Get("/", async(req, res) => await res.WriteAsync("Hello from Carter!"));
        }
    }
}
