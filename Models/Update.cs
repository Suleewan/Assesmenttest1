using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assesmentpaksod.Models
{
    public class Update
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string E_mail { get; set; }
        public string Phone { get; set; }
        public int User_Id { get; set; }
    }
    public class NameSubj
    {
        public string Name_Subj { get; set; }
        
    }
    public class UpdateSubject
    {
        public string Subject { get; set; }
        public int Sequence { get; set; }
        public int Sub_Id { get; set; }

    }
    public class UpdateAnswer
    {
        public string Answer { get; set;}
        public string Sequence { get; set; }
        public int Ans_Id { get; set;}
    }
}