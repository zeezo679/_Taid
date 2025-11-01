using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Interfaces;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminChatController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public AdminChatController(IMessageRepository messageRepository) {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messagesList = await _messageRepository.GetMessagesAsync();
            return View(messagesList);
        }
    }
}
