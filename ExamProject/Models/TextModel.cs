using ExamProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class TextModel
    {
        public int Text_ID { get; set; }
        public string Context { get; set; }

        internal static List<TextModel> GetTextList()
        {
            List<TextModel> _list = null;
            using(var context=new ExamProjectDbEntities())
            {
                _list = (from Text in context.Text
                         select new TextModel()
                         {
                             Text_ID=Text.Text_ID,
                             Context=Text.Context
                         }).ToList();
            }
            return _list;

        }
    }
}