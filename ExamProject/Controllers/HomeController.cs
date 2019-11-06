using ExamProject.App_Classes;
using ExamProject.Database;
using ExamProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<HtmlAgilityPack.HtmlNode> text_list = SinavModel.GetLast5Text();
            return View(text_list);
        }
        public ActionResult CreateExam(string QuestionUrl)
        {
           
            IEnumerable < HtmlAgilityPack.HtmlNode> text = SinavModel.GetWholeText(QuestionUrl);
            //IEnumerable<HtmlAgilityPack.HtmlNode> model = SinavModel.GetLast5Text();

            TempData["QuestionUrl"] = QuestionUrl;
            ViewBag.text = text;
            SinavModel model = new SinavModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateQuestion(SinavModel model)
        {
            var Text_Id = TempData["TextID"];
            int textId = (int)Text_Id;
            model.Create(textId);

            var QuestionUrl = TempData["QuestionUrl"];
            ViewBag.QuRl = QuestionUrl;
            return RedirectToAction("CreateExam",new { QuestionUrl});
        }
        [HttpPost]
        public ActionResult CreateExamContext(Text text)
        {
            Context.Baglanti.Text.Add(text);

            Context.Baglanti.SaveChanges();

            TempData["TextID"] = text.Text_ID;

            var QuestionUrl = TempData["QuestionUrl"];

            return RedirectToAction("CreateExam", new { QuestionUrl });
        }

        [Authorize]
        [HttpGet]
        public ActionResult ExamList()
        {
            List<TextModel> model = TextModel.GetTextList();
            return View(model);
        }

        public bool Kontrol(int id, string cevap)
        {
            bool sonuc = SinavModel.SonucuKontrolEt(id, cevap);
            if (sonuc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public ActionResult TakeExam(int tId)
        {
            List<SinavModel> questionList = SinavModel.GetTextQuestions(tId);
            ViewBag.TextQuestion = questionList;

            return View(questionList);
        }

        public bool ControlAnswer()
        {
            return true;
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        public ActionResult Footer()
        {
            return PartialView("~/Views/Shared/Partial/_Footer.cshtml");
        }
        public ActionResult TopMenu()
        {
            AspNetUserExtendedModel _model = new AspNetUserExtendedModel();

            //string _userId = User.Identity.GetUserId();
            //AspNetUsersExtendedModel _extUser = AspNetUsersExtendedModel.LoadModel(_userId);
            //_model.AvatarUrl = _extUser.ImagePath == null ? null : Path.Combine(PathConfig.GetRelativePath(SessionCorporateId, EPathConfig.SystemUsers), "thumbs", Path.GetFileName(_extUser.ImagePath));
            //_model.UserName = _extUser.Name + " " + _extUser.Surname;

            //CorporateModel _corporateModel = CorporateModel.LoadModel(SessionCorporateId);
            //_model.LogoPath = _corporateModel.ImagePathThumb;

            //byte _ruleValue = (byte)Session["Teklif_Satış_Yönetimi"];
            //List<string> AuthorizationGroupUserList = _ruleValue == (byte)EAuthorizationRuleValue.UstYetkiGrubu ? (List<string>)Session["UstYetkiGrubu"] : (List<string>)Session["AltYetkiGrubu"];

            //int _proposalCount;
            //_model.UnCheckedProposals = ProposalModel.GetLastProposalList(SessionCorporateId, _userId, _ruleValue, AuthorizationGroupUserList, (byte)EProposalStatus.Teklif, out _proposalCount, _extUser.LastProposalCheckDate);
            //_model.UnCheckedProposalCount = _proposalCount;

            //int _orderCount;
            //_model.UnCheckedOrders = ProposalModel.GetLastProposalList(SessionCorporateId, _userId, _ruleValue, AuthorizationGroupUserList, (byte)EProposalStatus.Satış, out _orderCount, _extUser.LastOrderCheckDate);
            //_model.UnCheckedOrderCount = _orderCount;


            //byte _taskRuleValue = (byte)Session["İş_Yönetimi"];
            //List<string> TaskAuthorizationGroupUserList = _taskRuleValue == (byte)EAuthorizationRuleValue.UstYetkiGrubu ? (List<string>)Session["UstYetkiGrubu"] : (List<string>)Session["AltYetkiGrubu"];
            //int myTaskCount, unCheckedTaskCount;
            //_model.UnCheckedTasks = TaskModel.GetLastTask(SessionCorporateId, _userId, _taskRuleValue, TaskAuthorizationGroupUserList, out myTaskCount, out unCheckedTaskCount, _extUser.LastTaskCheckDate);
            //_model.MyTaskCount = myTaskCount;
            //_model.UnCheckedTaskCount = unCheckedTaskCount;


            return PartialView("~/Views/Shared/Partial/_TopMenu.cshtml", _model);
        }
        public ActionResult LeftMenu()
        {
            return PartialView("~/Views/Shared/Partial/_LeftMenu.cshtml");
        }
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
