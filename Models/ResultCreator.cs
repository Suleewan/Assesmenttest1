using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Assesmentpaksod.Models
{
    public class ResultCreator
    {
        private List<ResultCreator> resultCreator;

        //internal object ResultCreator;
        public ResultCreator()
        {
            
        }

        public string Name_Subj { get; set; }
        public string Createdate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string E_mail { get; set; }
        public string Phone { get; set; }
        public int Sub_Id { get; set; }
        public int User_Id { get; set; }

        public List<ResultSubject> Subject { get; set; }
        
        public string Sequence { get; internal set; }
        
    }
    public class CustomerandServay
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string E_mail { get; set; }
        public string Phone { get; set; }
        public int User_Id { get; set; }
        public int TQ_Id { get; set; }
        
        public List<SubannAns> SubandAns { get; set; }

    }
    public class SubannAns
    {
        public int Sub_Id { get; set; }
        public int Ans_Id { get; set;}
        public string Other { get; set; }


    }
    
}