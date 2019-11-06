using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using HtmlAgilityPack;
using ExamProject.Database;

namespace ExamProject.Models
{
    public class SinavModel
    {
        public int QId { get; set; }
        public string Option_A { get; set; }
        public string Option_B { get; set; }
        public string Option_C { get; set; }
        public string Option_D { get; set; }
        public string Answer { get; set; }
        public int Text_ID { get; set; }
        public string Soru { get; set; }
        public string Text { get; set; }

        internal void Create(int textId)
        {
            using(var context=new ExamProjectDbEntities())
            {
                ExamProject.Database.Question _question = new Question()
                {
                    Option_A = Option_A,
                    Option_B = Option_B,
                    Option_C = Option_C,
                    Option_D = Option_D,
                    Text_ID = textId,
                    Soru = Soru,
                    Answer=Answer,
                };
                context.Entry(_question).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
        }
        internal static IEnumerable<HtmlNode> GetLast5Text()
        {
            Uri url = new Uri( "https://www.wired.com/");
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
            dokuman.LoadHtml(html);
            //HtmlNodeCollection basliklar = dokuman.DocumentNode.SelectNodes("//div[contains(@class, 'hello')]"));
            //foreach (var baslik in basliklar)
            //{

            //}
            IEnumerable<HtmlNode> nodes =dokuman.DocumentNode.Descendants(0).Where(n => n.HasClass("card-component__description")).Take(5);
            //*[@id="app-root"]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[1]/div/ul/li[2]/a[2]
            //IEnumerable<HtmlNode> nodes = dokuman.DocumentNode.SelectNodes("//*[@id='app-root']/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[1]/div/ul/li[2]/a[2]");
            return nodes;
        }

        internal static IEnumerable<HtmlNode>GetWholeText(string QuestionUrl)
        {
            QuestionUrl=QuestionUrl.Replace("%2F", "/");
            Uri url = new Uri("https://www.wired.com" + QuestionUrl);
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
            dokuman.LoadHtml(html);
            IEnumerable<HtmlNode> text = dokuman.DocumentNode.SelectNodes("//p").Take(5);
            return text;
        }
        internal static bool SonucuKontrolEt(int id,string cevap)
        {
            using (var context = new ExamProjectDbEntities())
            {
              Question _question = context.Question.Where(x => x.QId == id).FirstOrDefault();
                if (_question.Answer == cevap)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal static List<SinavModel> GetTextQuestions(int Text_Id)
        {
            List<SinavModel> _questionList = null;
            using(var context = new ExamProjectDbEntities())
            {
                _questionList = (from Quextion in context.Question
                                 join Text in context.Text on Quextion.Text_ID equals Text.Text_ID
                                 where Quextion.Text_ID == Text_Id
                                 select new SinavModel()
                                 {
                                     QId=Quextion.QId,
                                     Text=Text.Context,
                                     Soru=Quextion.Soru,
                                     Option_A=Quextion.Option_A,
                                     Option_B=Quextion.Option_B,
                                     Option_C=Quextion.Option_C,
                                     Option_D=Quextion.Option_D,
                                     Answer=Quextion.Answer,
                                 }).ToList();
                    
            }
            return _questionList;
        }
    }
}