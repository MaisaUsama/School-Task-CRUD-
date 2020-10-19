using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class StudentController : ApiController
    {
        SchoolContext db;
        public StudentController()
        {
            db = new SchoolContext();

        }
        string img = null;
        public IHttpActionResult GetStudents() {
            var Stds = db.Students.OrderByDescending(s=>s.id).ToList();
            if (Stds.Count > 0) { return Ok(Stds); }
            else
                return NotFound();
        
        
        }

        public IHttpActionResult GetStudent(int id) {
            var std = db.Students.FirstOrDefault(S => S.id == id);
            if(std is null)
            {
                return NotFound();
            }
            return Ok(std);
        }


        /* public IHttpActionResult PostStudent(Student student)
          { 
              if (student is null)
                  return BadRequest();
            //  student.image = img;
              db.Students.Add(student);
              db.SaveChanges();
              return Ok(student);
          }*/
        public IHttpActionResult PostStudent()
        {
            string imageName = null;
            var httpRequest = System.Web.HttpContext.Current.Request;
            //Upload Image
            string imagePath;
            var postedFile = httpRequest.Files["image"];
            if (postedFile != null)
            {

                //Create custom filename
                // imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                // imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                imageName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                 imagePath = "/Content/Images/" + imageName;


                imageName = System.Web.HttpContext.Current.Server.MapPath(imagePath);
                postedFile.SaveAs(imageName);
          
            
            }
            else
            {
                imagePath = "Content/Images/Defualt.jpg";
                imageName = System.Web.HttpContext.Current.Server.MapPath(imagePath);
                postedFile.SaveAs(imageName);
            }

            //  s.image = imageName;
            //Save to DB
            //  using (DBModel db = new DBModel())
            // {
            Student image = new Student()
            {
             Name = httpRequest["Name"],
              Age =int.Parse(httpRequest["Age"]),
             Email= httpRequest["Email"],
                image = imagePath

                /* Name = s.Name,
                Age = s.Age,
                Email = s.Email,
                image =s.image*/
            };
                db.Students.Add(image);
                db.SaveChanges();
            //  }
            return Ok();
        }


        /*   public IHttpActionResult PutStudent([FromUri]int id,  [FromBody]Student Newstudent) {

               if (Newstudent is null)
               {
                   return BadRequest("BaaaaD");
               }
                   var OldStudent = db.Students.FirstOrDefault(S => S.id == id);

               if (OldStudent == null)
               {
                   return NotFound();
               }
             //  db.Entry(Newstudent).State = System.Data.Entity.EntityState.Modified;
               OldStudent.Name = Newstudent.Name;
               //OldStudent.ID = Newstudent.ID;
               OldStudent.Age= Newstudent.Age;
               OldStudent.Email = Newstudent.Email;
               db.SaveChanges();
               return Ok();

           }*/


        //



        public IHttpActionResult PutStudent(int id)
        {
            string imageName = null;
            var httpRequest = System.Web.HttpContext.Current.Request;
            //Upload Image
           /* string imagePath;
            var postedFile = httpRequest.Files["image"];
                //Create custom filename
                // imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                // imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                imageName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                imagePath = "/Content/Images/" + imageName;


                imageName = System.Web.HttpContext.Current.Server.MapPath(imagePath);
                postedFile.SaveAs(imageName);
            
            */
            var OldStudent = db.Students.FirstOrDefault(S => S.id == id);

            if (OldStudent == null)
            {
                return NotFound();
            }
            OldStudent.Name = httpRequest["Name"];
            OldStudent.Age = int.Parse(httpRequest["Age"]);
            OldStudent.Email = httpRequest["Email"];
            //OldStudent.image = imagePath;
            db.SaveChanges();
            return Ok();

        }


        public IHttpActionResult DeleteStudent(int id) {

            var std = db.Students.FirstOrDefault(S => S.id == id);
            if (std is null)
                return NotFound();
            else
                db.Students.Remove(std);
            db.SaveChanges();
            return Ok();
        
        }
        /*  [HttpPost]
          public IHttpActionResult upload() {

              string image = null;
              var httpRequest = System.Web.HttpContext.Current.Request;
              var postedFile = httpRequest.Files["Image"];
              image = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", " - ");
              image = image + DateTime.Now.ToString("tata") + Path.GetExtension(postedFile.FileName);

              var filePath = System.Web.HttpContext.Current.Server.MapPath("~/image/" + image);
              postedFile.SaveAs(filePath);



              db.SaveChanges();

              return Ok();
          }*/
        [HttpPost]
        [Route("api/EditImage")]
        public IHttpActionResult EditImage(int id )
        {
            string imageName = null;
            var httpRequest = System.Web.HttpContext.Current.Request;
            string imagePath;
            var postedFile = httpRequest.Files["image"];
            imageName = Path.GetFileNameWithoutExtension(postedFile.FileName);
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            imagePath = "/Content/Images/" + imageName;
            imageName = System.Web.HttpContext.Current.Server.MapPath(imagePath);
            postedFile.SaveAs(imageName);
            var user = db.Students.Find(id);
            user.image = imagePath;
            db.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Route("api/GetImage")]
        public string GetImage(int id )
        {
            string x = "NoData";
                var user = db.Students.Find(id);
                var photo = user.image;
            if(photo != null)
            return photo;
            return x;


        }
    }
}
