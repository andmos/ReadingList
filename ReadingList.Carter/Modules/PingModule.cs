using Carter;
using Carter.Response;

namespace ReadingList.Web.Modules
{
    public class PingModule : CarterModule
    {
        public PingModule() : base("/api")
        {
            Get("/ping", async (req, res) =>
            {
                await res.AsJson("pong");
            });
        }
    }
}