using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using MimeKit;


using ScopeIndia.Models;
using System.Diagnostics;

using MailKit.Net.Smtp;
using ScopeIndia.Data;

using MailKit.Security;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;





namespace ScopeIndia.Controllers
{
    public class WebHomeController : Controller
    {
        
        private readonly ILogger<WebHomeController> _logger;
        private readonly IStudent _student;
        private readonly ICourse _course;
        private readonly CourseSql _courseSql;
        private readonly StudentSql _studentsql;
       

        public WebHomeController(ILogger<WebHomeController> logger, IStudent student,StudentSql studentsql,ICourse course,CourseSql coursesql)
        {
           
            _logger = logger;
            _student = student;
            _studentsql = studentsql;
            _course = course;
            _courseSql=coursesql;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Aboutus()
        {

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactModel contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(contact.FromAddress));
            email.To.Add(MailboxAddress.Parse(contact.ToAddress));
            email.Subject = $"Subject: {contact.Subject}";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"From: {contact.FromAddress}" + "\n" + $"From: {contact.ToAddress}" + "\n" + $"From: {contact.Subject}" + "\n" + $"From: {contact.Message}" + "\n"
            };

            var smtp = new SmtpClient(); //mail send pana variable create pandrom
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("asha.kashini@gmail.com", "mgxz mtnp rfht cjll");
            smtp.Send(email);
            smtp.Disconnect(true);

            ViewBag.Email = "A mail has been sent successfully";
            return View("Contact");
        }

        public IActionResult Student()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Student(StudentModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            student.Avatarpath = Upload(student.StudentUploadAvatar);
            _student.Insert(student);
            StudentModel student1 = _student.GetByEmail(student.StudentEmail);
            //string studentEmail = student.StudentEmail;
            string url = $"https://localhost:7120/WebHome/EmailVerification/{student.Id}";
            var email = new MimeMessage();
            //Console.WriteLine(student.StudentEmail);
            email.To.Add(MailboxAddress.Parse((String)student.StudentEmail));
            email.From.Add(MailboxAddress.Parse((string)student.StudentEmail));
            //string validationCode = GenerateValidationCode();
            

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                //Text = $"{url}"
                Text = $@"
                <html>
                <head>
                <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.5; }}
                .container {{ text-align: center; padding: 20px; }}
                .button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                color: #ffffff;
                background-color: #007bff;
                text-decoration: none;
                border-radius: 5px;
                }}
                .code {{ font-weight: bold; font-size: 18px; color: #333; }}
                </style>
                </head>
                <body>
                <div class='container'>
                <h3>Welcome to Our Service!</h3>
                <p>Thank you for registering with us. Please confirm your email by clicking the button below:</p>
                <p><a href='https://localhost:7120/WebHome/EmailVerification/{student1.Id}' class='button'>Confirm Your Email</a></p>
                <p>If the link doesn't work, please copy and paste this code into the verification page:</p>
        
                </div>
                </body>
                </html>"
                };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("asha.kashini@gmail.com", "mgxz mtnp rfht cjll"); // Replace with secure credentials
            smtp.Send(email);
            smtp.Disconnect(true);

            ViewBag.Email = "A mail has been sent successfully";
            return RedirectToAction("Login");

        }


        [Route("/WebHome/EmailVerification/{Id}")]
        public IActionResult EmailVerification(int Id)
        {
            StudentModel student = _student.GetById(Id);
            if (student == null)
            {
                return View("Home");
            }
            student.IsVerified = true;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("asha.kashini@gmail.com"));
            email.To.Add(MailboxAddress.Parse(student.StudentEmail));
            email.Subject = $"Subject:EmailVerification";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "Thank you for Registering with us"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("asha.kashini@gmail.com", "mgxz mtnp rfht cjll"); // Replace with secure credentials
            smtp.Send(email);
            smtp.Disconnect(true);
            ViewBag.EmailValidationMessage = "Email Successfully Validated";
            return View("Login");


        }

        public IActionResult Login()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult Course()
        {

            var course=_courseSql.GetAll();
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel login)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(login);
            //}

            StudentModel student = _student.GetByEmail(login.Email);
            if (student == null || login.Password != student.password)
            {
                ViewBag.LoginError = "Invalid Email or Password";
                return View();
            }
            //HttpContext.Session.SetInt32("UserId", student.Id);
            HttpContext.Session.SetString("UserEmail", student.StudentEmail);
            HttpContext.Session.SetString("UserName", student.StudentFirstName);
            HttpContext.Session.SetString("UserAvatar", student.Avatarpath);

            //Console.WriteLine("login"+ login.KeepMeLoggedIn);

            if (login.KeepMeLoggedIn)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    Secure = true,
                    HttpOnly = true,
                    SameSite=SameSiteMode.Strict,
                    Path="/"
                };
                HttpContext.Response.Cookies.Append("UserStatus", "login", options);
                HttpContext.Response.Cookies.Append("UserId", student.Id.ToString(),options);
                HttpContext.Response.Cookies.Append("UserEmail", student.StudentEmail,options);
                HttpContext.Response.Cookies.Append("UserName", student.StudentFirstName.ToString(),options);
            }


            TempData["Student"] = student.StudentEmail;
            return RedirectToAction("Dashboard");
        }
        
        public IActionResult ForgotPassword()
        {
            return View();  
        }


        [HttpPost]
        public IActionResult ForgotPassword(string studentEmail,PasswordSettingModel password)
        {
            if (!TempData.ContainsKey("PasswordSetting") || TempData["PasswordSetting"] == null)
            {
                ViewBag.PasswordError = "Session Expired.Please Request a new OTP";
                return RedirectToAction("ForgotPassword");
            }
            string email = TempData["PasswordSetting"].ToString();
            //if (!ModelState.IsValid)
            //{
            //    return View("forgot");
            //}
            if (password.password != password.ConfirmPassword)
            {
                ViewBag.PasswordError = "Password do not match.Try again";
                return View();
            }
            StudentModel student = _studentsql.GetByEmail(studentEmail);
            if (student == null)
            {
                ViewBag.PasswordMatch = "User not found.Please Register";
                return RedirectToAction("Registeration");
            }
            student.password = password.password;
            _studentsql.Update(student);
            ViewBag.PasswordSuccess = "Password set Successfully! You can login now.";


            return RedirectToAction("Login");
           
        }


        public IActionResult FirstLogin()
        {
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FirstLogin(string email1, FirstLoginModel first)
        {

            if (string.IsNullOrEmpty(email1))
            {
                ViewBag.ErrorMessage = "Email is Required";
                return View();
            }
            StudentModel student = _studentsql.GetByEmail(email1);

            // Check if the email exists in the database
            if (student == null || !_studentsql.IsEmailExists(email1))
            {
                ViewBag.EmailValidationMessage = "Enter a Vlid Email Address";
                return View();
            }
            else
            {
                var Otp = GenerateValidationCode();
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("asha.kashini@gmail.com"));
                email.To.Add(MailboxAddress.Parse(email1));
                email.Subject = "Your OTP Code";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = $"Your OTP code is: {Otp}"
                };
                var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("asha.kashini@gmail.com", "mgxz mtnp rfht cjll");//"blxq myfw fjoa tbuc"
                smtp.Send(email);
                smtp.Disconnect(true);
                TempData["OTP"] = Otp;
                TempData["Email"] = email1;
                return RedirectToAction("OtpVerification");
            }
        }
        private string GenerateValidationCode()
        {
            // Generate a random 6-digit OTP
            Random rand = new Random();
            string code = rand.Next(100000, 999999).ToString(); // 6-digit random number
            return code;
        }
        public IActionResult OtpVerification()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OtpVerification(string otp)
        {
            try
            {
                string checkOtp = TempData["otp"].ToString();

                if (string.IsNullOrEmpty(checkOtp))
                {
                    ViewBag.OTPMismatch = "OTP has expired or is missing. Please try again.";
                    return RedirectToAction("Login");
                }

                if (otp == checkOtp)
                {
                    if (TempData.ContainsKey("Email"))

                        TempData["PasswordSetting"] = TempData["Email"].ToString();

                    else
                        return RedirectToAction("Login");


                    return RedirectToAction("Passwordsetting");
                }
                else
                {
                    ViewBag.OTPMismatch = "OTP entered doesn't match. Try again.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.OTPMismatch = $"Something went wrong. Please try again. Error: {ex.Message}";
                return RedirectToAction("Login");
            }
        }


        public IActionResult PasswordSetting()
        {

            if (!TempData.ContainsKey("PasswordSetting"))
            {
                return RedirectToAction("Login");
            }
            ViewBag.Email = TempData["PasswordSetting"];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordSetting(PasswordSettingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model); // Show validation errors
                }

                string email = model.Email;
                StudentModel student = _studentsql.GetByEmail(email);

                if (student == null)
                {
                    ViewBag.Error = "Student not found with the provided email.";
                    return View(model);
                }

                // Check if password is already set
                if (!string.IsNullOrEmpty(student.password))
                {
                    TempData["PasswordSetAlert"] = "Password is already set. Please login.";
                    return RedirectToAction("Login");
                }

                // Set the new password
                student.password = model.password; // Consider hashing here
                _studentsql.Update(student);

                TempData["SuccessMessage"] = "Password set successfully. Please login.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Something went wrong: " + ex.Message;
                return View(model);
            }
        }


  

        public IActionResult DashboardJoin()
        {
    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DashboardJoin( int Sid)
        {
            StudentModel student =_studentsql.GetById(Sid);
            student.CourseId = null;
            _studentsql.Update(student);
            TempData["Sid"]= student.Id.ToString();
            return View("Login");
        }


      

        public IActionResult Dashboard(bool changeCourse = false)
        {
            var email = HttpContext.Session.GetString("UserEmail") ?? Request.Cookies["UserEmail"];
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var student = _studentsql.GetByEmail(email);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.Avatar = student.Avatarpath;
            ViewBag.Name = student.StudentFirstName;
            ViewBag.Cid = student.CourseId;
            ViewBag.Sid = student.Id;

            if(student.CourseId != null)
            {
                var joinedCourse = _courseSql.GetById(student.CourseId.Value);
                ViewBag.JoinedCourse = joinedCourse;
            }


            return View("DashboardJoin", student); // default joined view
        }


        
        [HttpGet]
        public IActionResult JoinCourse(int courseId)
        {
            var course = _courseSql.GetById(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new JoinCourseModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Duration = course.Duration,
                Fee = course.Fee
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmJoin(int courseId)
        {
            var email = HttpContext.Session.GetString("UserEmail") ?? Request.Cookies["UserEmail"];
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var student = _studentsql.GetByEmail(email);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // ? Now store the course ID
            student.CourseId = courseId;
            _studentsql.Update(student);

            TempData["Success"] = "You have successfully joined the course!";
            return RedirectToAction("Dashboard");
        }

        public IActionResult EditProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail") ?? Request.Cookies["UserEmail"];
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var student = _studentsql.GetByEmail(email);
            if (student == null)
                return NotFound();

            // Prepopulate Gender & Hobbies for the view (if needed)
            ViewBag.Gender = student.Gender; // e.g. "male", "female", "other"
            ViewBag.Hobbies = student.StudentHobbies?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string>();

            return View(student);
        }

        // POST: Save edited data + avatar upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(StudentModel model, IFormFile StudentUploadAvatar)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, send Gender & Hobbies back
                ViewBag.Gender = model.Gender;
                ViewBag.Hobbies = model.StudentHobbies?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string>();
                return View(model);
            }

            var student = _studentsql.GetByEmail(model.StudentEmail);
            if (student == null)
                return NotFound();

            // Update student fields
            student.StudentFirstName = model.StudentFirstName;
            student.StudentLastName = model.StudentLastName;
            student.Gender = model.Gender;
            student.StudentDateOfBirth = model.StudentDateOfBirth;
            student.StudentPhoneNumber = model.StudentPhoneNumber;
            student.StudentCountry = model.StudentCountry;
            student.StudentState = model.StudentState;
            student.StudentCity = model.StudentCity;
            student.StudentHobbies = model.StudentHobbies;

            // Handle Avatar Upload
            if (StudentUploadAvatar != null && StudentUploadAvatar.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(StudentUploadAvatar.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await StudentUploadAvatar.CopyToAsync(fileStream);
                }
                student.Avatarpath = uniqueFileName;
            }

            _studentsql.Update(student); // Save changes to DB

            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Dashboard");
        }
    
    public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserEmail");
            return RedirectToAction("Home");
        }

        public IActionResult PasswordSetSuccess()
        {
            var model = new PasswordSuccessModel
            {
                Email = "user@example.com",        // replace with real data
                UserName = "JohnDoe"               // replace with real data
            };

            return View(model);
        }

        public string? Upload(IFormFile myfile)
        {
            string? upload_path = null;

            string File_Name = myfile.FileName;
            File_Name = Path.GetFileName(File_Name);
            string upload_folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
            if (!Directory.Exists(upload_folder))
            {
                Directory.CreateDirectory(upload_folder);
            }

             upload_path = Path.Combine(upload_folder, File_Name);
            try
            {
                if (!File_Name.Except(upload_path).Any())
                {
                    var uploadsteam = new FileStream(upload_path, FileMode.Create);
                     myfile.CopyTo(uploadsteam);
                    ViewBag.AvatarMessage = "Upload Success";
                }
            }
            catch (IOException FailureMessage)
            {
                ViewBag.AvatarMessage = "Upload Failure. Please try again";


            }

            return File_Name;
        }

       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
