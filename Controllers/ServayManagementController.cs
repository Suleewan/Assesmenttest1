using Assesmentpaksod.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System;
using System.Web.Configuration;
using System.Web.Http.Results;
using System.Collections.Concurrent;
using System.Windows.Documents;
using Microsoft.Ajax.Utilities;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Net.Http;


namespace Assesmentpaksod.Controllers
{
    public class ServayManagementController : ApiController
    {
        private Entities _dbContext = new Entities();

        //public IHttpActionResult View { get; private set; }




        //GET: ServayManagement
        //GET: api/ServayManagement
        public List<ResultCreator> GetAllServay()
        {
            var objSubject = _dbContext.M_Subject
                .Select(s => new ResultSubject
                {
                    Sub_Id = s.Sub_Id,
                    Sequence = s.Sequence.Value,
                    Subject = s.Subject,
                    Answers = _dbContext.M_Answer.Where(x => x.Sub_Id == s.Sub_Id)
                                                 .Select(se => new ResultAnswer
                                                 {
                                                     Sub_Id = se.Sub_Id,
                                                     Sequence = se.Sequence,
                                                     Answer = se.Answer,
                                                     Ans_Id = se.Ans_Id,
                                                 }).ToList(),
                }).ToList();

            var result = _dbContext.T_Questionservay
                .Select(s => new ResultCreator
                {
                    Name_Subj = s.T_Question.Name_Subj,
                    FirstName = s.M_Customers.FirstName,
                    LastName = s.M_Customers.LastName,
                    Address = s.M_Customers.Address,
                    E_mail = s.M_Customers.E_mail,
                    Phone = s.M_Customers.Phone,
                    Sub_Id = s.Sub_Id

                }).ToList();
            foreach (var item in result)
            {
                item.Subject = objSubject.Where(x => x.Sub_Id == item.Sub_Id).ToList();
            }
            return result;

        }


        //[ResponseType(typeof(ResultCreator))]
        [HttpPost]
        [Route("CreateSubjectAndAnswer")]
        public IHttpActionResult CreateSubjectAndAnswer(List<ResultSubject> Objs)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    using (var dbContext = new Entities())
                    {
                        var Subjs = new List<Assesmentpaksod.M_Subject>();
                        foreach (var item in Objs)
                        {
                            var SubObj = new M_Subject();
                            SubObj.Subject = item.Subject;
                            SubObj.Sequence = item.Sequence;
                            _dbContext.M_Subject.Add(SubObj);
                            _dbContext.SaveChanges();

                            foreach (var itemAns in item.Answers)
                            {
                                var AnsObj = new M_Answer();
                                AnsObj.Sequence = itemAns.Sequence;
                                AnsObj.Sub_Id = SubObj.Sub_Id;
                                AnsObj.Answer = itemAns.Answer;
                                _dbContext.M_Answer.Add(AnsObj);
                            }
                            _dbContext.SaveChanges();
                        }
                    }
                    // If everything succeeded so far, complete the transaction
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occurred during database operations
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Ok(false);
                }
            }
            return Ok(true);
        }
        [HttpPost]
        [Route("CreatQuestion")]
        public IHttpActionResult CreatQuestion(QuestionDataRequest question)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    using (var dbContext = new Entities())
                    {
                        Console.WriteLine("Test");
                        var obj = new T_Question();
                        obj.Name_Subj = question.Name_Subj;
                        obj.Createdate = DateTime.Now;
                        _dbContext.T_Question.Add(obj);
                        _dbContext.SaveChanges();

                        foreach (var item in question.Sub_Id)
                        {
                            var objDetail = new T_QuestionDetail();
                            objDetail.Sub_Id = item;
                            objDetail.TQ_Id = obj.TQ_Id;
                            _dbContext.T_QuestionDetail.Add(objDetail);
                        }
                        _dbContext.SaveChanges();
                    }
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Ok(ex.Message);
                }
                return Ok(true);

            }
        }

        [HttpPost]
        [Route("DataSubjandAnsw")]
        public IHttpActionResult DataSubjandAnsw(CustomerandServay cusservay)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    using (var dbContext = new Entities())
                    {
                       
                        var customer = new M_Customers();
                        customer.FirstName = cusservay.FirstName;
                        customer.LastName = cusservay.LastName;
                        customer.Address = cusservay.Address;
                        customer.E_mail = cusservay.E_mail;
                        customer.Phone = cusservay.Phone;
                        
                        _dbContext.M_Customers.Add(customer);
                        _dbContext.SaveChanges();

                        foreach (var item in cusservay.SubandAns)
                        {
                            var servey = new T_Questionservay();
                            servey.TQ_Id = cusservay.TQ_Id;
                            servey.User_Id = customer.User_Id;
                            servey.Sub_Id = item.Sub_Id;
                            servey.Ans_Id = item.Ans_Id;
                            servey.Other = item.Other;
                            _dbContext.T_Questionservay.Add(servey);
                        }
 
                        _dbContext.SaveChanges();
                    }
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return Ok(ex.Message);
                }
                return Ok(true);

            }
        }


        [HttpPut]
        [Route("UpdateNameSubj")]
        public IHttpActionResult UpdateNameSubj(int id, NameSubj UpdateNameSubj)
        {
            try
            {
                // ค้นหาข้อมูล Subject ที่ต้องการอัปเดตด้วย ID ที่ระบุ
                var existingNameSubj = _dbContext.T_Question.FirstOrDefault(s => s.TQ_Id == id);
                if (existingNameSubj == null)
                {
                    return NotFound(); // ถ้าไม่พบข้อมูล Subject ที่ต้องการอัปเดต
                }

                // อัปเดตข้อมูลในตาราง M_Subject
                existingNameSubj.Name_Subj = UpdateNameSubj.Name_Subj;
                             
                _dbContext.SaveChanges();

                // คืนค่า HTTP status 200 (OK) เมื่ออัปเดตข้อมูลสำเร็จ
                return Ok("NameSubj updated successfully.");
            }
            catch (Exception ex)
            {
                // หากเกิดข้อผิดพลาดขณะดำเนินการอัปเดต คืนค่า HTTP status 500 (Internal Server Error) พร้อมข้อความข้อผิดพลาด
                return InternalServerError(ex);
            }
        }
        [HttpPut]
        [Route("UpdateSubject")]
        public IHttpActionResult UpdateSubject(int id, UpdateSubject UpdateSubject)
        {
            try
            {
                // ค้นหาข้อมูล Subject ที่ต้องการอัปเดตด้วย ID ที่ระบุ
                var existingSubject = _dbContext.M_Subject.FirstOrDefault(s => s.Sub_Id == id);
                if (existingSubject == null)
                {
                    return NotFound(); // ถ้าไม่พบข้อมูล Subject ที่ต้องการอัปเดต
                }

                // อัปเดตข้อมูลในตาราง M_Subject
                existingSubject.Subject = UpdateSubject.Subject;
                existingSubject.Sequence = UpdateSubject.Sequence;
                _dbContext.SaveChanges();

                // คืนค่า HTTP status 200 (OK) เมื่ออัปเดตข้อมูลสำเร็จ
                return Ok("Subject updated successfully.");
            }
            catch (Exception ex)
            {
                // หากเกิดข้อผิดพลาดขณะดำเนินการอัปเดต คืนค่า HTTP status 500 (Internal Server Error) พร้อมข้อความข้อผิดพลาด
                return InternalServerError(ex);
            }
        }
        [HttpPut]
        [Route("UpdateAnswers")]
        public IHttpActionResult UpdateAnswers(int id, UpdateAnswer UpdateAnswer)
        {
            try
            {
                // ค้นหาข้อมูล Subject ที่ต้องการอัปเดตด้วย ID ที่ระบุ
                var existingAnswer = _dbContext.M_Answer.FirstOrDefault(s => s.Ans_Id == id);
                if (existingAnswer == null)
                {
                    return NotFound(); // ถ้าไม่พบข้อมูล Subject ที่ต้องการอัปเดต
                }

                // อัปเดตข้อมูลในตาราง M_Subject
                existingAnswer.Answer = UpdateAnswer.Answer;
                existingAnswer.Sequence = UpdateAnswer.Sequence;
                _dbContext.SaveChanges();

                // คืนค่า HTTP status 200 (OK) เมื่ออัปเดตข้อมูลสำเร็จ
                return Ok("Answer updated successfully.");
            }
            catch (Exception ex)
            {
                // หากเกิดข้อผิดพลาดขณะดำเนินการอัปเดต คืนค่า HTTP status 500 (Internal Server Error) พร้อมข้อความข้อผิดพลาด
                return InternalServerError(ex);
            }
        }


        [HttpDelete]
        [Route("DeleteNameSubj/{id}")]
        public IHttpActionResult DeleteNameSubj(int id)
        {
            try
            {
                var questionDetailsToDelete = _dbContext.T_QuestionDetail.Where(d => d.TQ_Id == id);
                _dbContext.T_QuestionDetail.RemoveRange(questionDetailsToDelete);
                _dbContext.SaveChanges();

                var NameSubjToDelete = _dbContext.T_Question.FirstOrDefault(n => n.TQ_Id == id);
                if (NameSubjToDelete == null)
                {
                    return NotFound();
                }
                _dbContext.T_Question.Remove(NameSubjToDelete);
                _dbContext.SaveChanges();
                return Ok("NameSubject deleted successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }
        [HttpDelete]
        [Route("DeleteSubjandAnswer")]//delete
        public IHttpActionResult DeleteSubjandAnswer(int id, ResultSubject resultSubject)
        {
            try
            {
                // ค้นหา Subject ที่ต้องการลบโดยใช้ ID
                var subjectToDelete = _dbContext.M_Subject.FirstOrDefault(s => s.Sub_Id == id);
                if (subjectToDelete == null)
                {
                    return NotFound(); // ถ้าไม่พบ Subject ที่ต้องการลบ
                }
                var TqservayToDelete = _dbContext.T_Questionservay.Where(t => t.Sub_Id == id);
                _dbContext.T_Questionservay.RemoveRange(TqservayToDelete);

                var TqdetsilToDelete = _dbContext.T_QuestionDetail.Where(d => d.Sub_Id == id);
                _dbContext.T_QuestionDetail.RemoveRange(TqdetsilToDelete);
                // ค้นหา Answers ที่เกี่ยวข้องกับ Subject นี้และลบทั้งหมด
                var answersToDelete = _dbContext.M_Answer.Where(a => a.Sub_Id == id);
                _dbContext.M_Answer.RemoveRange(answersToDelete);

                // ลบ Subject ออกจากฐานข้อมูล
                _dbContext.M_Subject.Remove(subjectToDelete);

                // บันทึกการเปลี่ยนแปลงลงในฐานข้อมูล
                _dbContext.SaveChanges();

                // คืนค่า HTTP status 200 (OK) เมื่อลบข้อมูลสำเร็จ
                return Ok("Subject and answers deleted successfully.");
            }
            catch (Exception ex)
            {
                // หากเกิดข้อผิดพลาดขณะดำเนินการลบ คืนค่า HTTP status 500 (Internal Server Error) พร้อมข้อความข้อผิดพลาด
                return InternalServerError(ex);
            }
        }

    
    
        private bool ResultCreatorExists(int id)
        {
            throw new NotImplementedException();
        }

        private List<ResultCreator> BadRequest(ResultCreator modelState)
        {
            return new List<ResultCreator>();
        }


        private List<ResultCreator> PostAllServays()
        {

            return new List<ResultCreator>();
        }

    }

}










