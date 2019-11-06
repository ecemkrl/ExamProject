using ExamProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamProject.App_Classes
{
    public class Context
    {
      private static ExamProjectDbEntities baglanti;
      public static ExamProjectDbEntities Baglanti
        {
            get
            {
                if (baglanti == null)
                    baglanti = new ExamProjectDbEntities();
                return baglanti;
            }
            set { Baglanti = value; }
        }
    }
}