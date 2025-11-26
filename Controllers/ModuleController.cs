using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class ModulesController : Controller
{
    // 免费模块，所有用户都可以访问
    public IActionResult Free()
    {
        return View(); // 返回 Views/Modules/Free.cshtml
    }

    // 高级模块，需要会员
    [Authorize] // 确保用户已登录
    public IActionResult Premium()
    {
        bool isPremium = User.IsInRole("Premium"); // 或者数据库字段判断
        if (!isPremium)
        {
            // 非会员跳到升级提示页面
            return RedirectToAction("Upgrade");
        }

        return View(); // 返回 Views/Modules/Premium.cshtml
    }

    // 提示用户升级会员
    [Authorize]
    public IActionResult Upgrade()
    {
        return View(); // 返回 Views/Modules/Upgrade.cshtml
    }
}
