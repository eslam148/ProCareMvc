using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Core;
using PayPalHttp;
using ProCareMvc.business;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayPalOrders = PayPalCheckoutSdk.Orders;

namespace ProCareMvc.Presentation.Controllers
{

    public class PaymentController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly string PayPalClientId;
        private readonly string PayPalSecret;
        private readonly string ReturnUrl;
        private readonly string CancelUrl;

        public PaymentController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            UnitOfWork = unitOfWork;
            PayPalClientId = configuration["PayPal:ClientId"];
            PayPalSecret = configuration["PayPal:Secret"];
            ReturnUrl = configuration["PayPal:ReturnUrl"];
            CancelUrl = configuration["PayPal:CancelUrl"];
        }

    
    
        

        public async Task<IActionResult> Book(string startTime, Guid doctorId)
        {
            if (!DateTime.TryParse(startTime, out DateTime appointmentTime))
            {
                return BadRequest("Invalid time format.");
            }

            var doctor = await UnitOfWork.Doctor.GetAll()
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            var isBooked = await UnitOfWork.Appointment.GetAll()
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.StartTime == appointmentTime &&
                               !a.IsDeleted);
            if (isBooked)
            {
                TempData["Error"] = "This appointment is already booked.";
                return RedirectToAction("Details", "Home", new { id = doctorId });
            }

            var model = new BookAppointmentVM
            {
                DoctorId = doctorId,
                DoctorName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                StartTime = appointmentTime,
                Amount = 50.00
            };

            ViewBag.PayPalClientId = PayPalClientId;
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> CreatePayPalOrder([FromBody] BookAppointmentVM model)
        {
            try
            {
                var environment = new SandboxEnvironment(PayPalClientId, PayPalSecret);
                var client = new PayPalHttpClient(environment);

                var order = new PayPalOrders.OrderRequest
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PayPalOrders.PurchaseUnitRequest>
            {
                new PayPalOrders.PurchaseUnitRequest
                {
                    AmountWithBreakdown = new PayPalOrders.AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        Value = model.Amount.ToString("F2")
                    }
                }
            },
                    ApplicationContext = new PayPalOrders.ApplicationContext
                    {
                        ReturnUrl = ReturnUrl,
                        CancelUrl = CancelUrl
                    }
                };

                var request = new PayPalOrders.OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(order);

                var response = await client.Execute(request);
                var result = response.Result<PayPalOrders.Order>();

                return Json(new { orderId = result.Id });
            }
            catch (Exception ex)
            {
                // Log the actual error in the console or log system
                Console.WriteLine("Error in CreatePayPalOrder: " + ex.Message);
                return Json(new { error = "An error occurred, please try again." });
            }
        }


        public async Task<IActionResult> Complete(string orderId, Guid doctorId, string startTime)
        {
            if (!DateTime.TryParse(startTime, out DateTime appointmentTime))
            {
                return BadRequest("Invalid time format.");
            }

            try
            {
                var environment = new SandboxEnvironment(PayPalClientId, PayPalSecret);
                var client = new PayPalHttpClient(environment);

                var request = new PayPalOrders.OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");

                var response = await client.Execute(request);
                var result = response.Result<PayPalOrders.Order>();

                if (result.Status == "COMPLETED")
                {
                    // 1. إنشاء الموعد
                    var appointment = new Appointment
                    {
                        DoctorId = doctorId,
                        StartTime = appointmentTime,
                        EndTime = appointmentTime.AddMinutes(30),
                        PatienId = Guid.Empty,
                        IsDeleted = false,
                        Name = "Consultation"
                        // OrderItemId بيفضل NULL
                    };
                    await UnitOfWork.Appointment.InsertAsync(appointment);

                    // 2. إنشاء الطلب
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        PatientId = Guid.Empty,
                        PaymentMethod = "PayPal",
                        TotalPrice = 50,
                        DateOrder = DateTime.Now,
                        Status = "Completed"
                    };
                    await UnitOfWork.Order.InsertAsync(order);

                    // 3. إنشاء عنصر الطلب
                    var orderItem = new AppointmentOrderItem
                    {
                        Id = Guid.NewGuid(),
                        Order = order,
                        AppointmetnId = appointment.Id
                    };
                    await UnitOfWork.OrderItem.InsertAsync(orderItem);

                    // حفظ التغييرات
                    UnitOfWork.Save();

                    TempData["Success"] = "Appointment booked successfully!";
                    return RedirectToAction("Details", "Home", new { id = doctorId });
                }

                TempData["Error"] = "Payment failed.";
                return RedirectToAction("Details", "Home", new { id = doctorId });
            }
            catch
            {
                TempData["Error"] = "An error occurred during payment.";
                return RedirectToAction("Details", "Home", new { id = doctorId });
            }
        }

        public IActionResult Cancel(Guid doctorId)
        {
            TempData["Error"] = "Payment was cancelled.";
            return RedirectToAction("Details", "Home", new { id = doctorId });
        }
    }
}