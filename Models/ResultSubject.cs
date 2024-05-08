using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assesmentpaksod.Models
{
    public class ResultSubject
    {
        public ResultSubject()
        {
            Answers = new List<ResultAnswer>();
        }
        public int TQ_Id { get; set; }
        public int Sequence { get; set; }
        public int Sub_Id { get; set; }
        public string Subject { get; set; }
        public List<ResultAnswer> Answers { get; set; }
     
    }

    public class QuestionDataRequest
    {
        public QuestionDataRequest()
        {
            Sub_Id = new List<int>();
        }
        public string Name_Subj { get; set; }
        public List<int> Sub_Id { get; set; }
    }
    
}