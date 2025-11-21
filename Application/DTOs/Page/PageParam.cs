
namespace Sindika.AspNet.app015.Application.DTOs.Page
{
    public class PageParam
    {
        public Guid ProfileId { get; set; }
        public List<PageEventChangeDTO> ChangedPageEvents { get; set; } = [];
    }
}
