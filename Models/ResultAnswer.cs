using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Assesmentpaksod.Models
{
    public class ResultAnswer
    {
        public ResultAnswer()
        {
            
        }
       
        public int Ans_Id { get; set; }
        public string Answer { get; set; }
        public int Sub_Id { get; set; }
        public string Sequence { get; set; }
      
       
    }
}
