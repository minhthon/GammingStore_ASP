using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Areas.Admin.Models;
using DoAnASP.Data;

namespace DoAnASP.Areas.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Payment_APIController : ControllerBase
    {
        private readonly DpContext _context;

        public Payment_APIController(DpContext context)
        {
            _context = context;
        }

        public class PaymentUpdate
        {
            public int ID { get; set; }
            public int Status { get; set; }
        }

        [HttpPost]
        public string UpdateStatusCancelOther(PaymentUpdate payment)
        {
            (from p in _context.Payment
             where p.IDPayment == payment.ID
             select p).ToList().ForEach(x => x.Status =3);
            _context.SaveChanges();
           return "Success";
        }
        public string UpdateStatusOrderConfirmation(PaymentUpdate payment)
        {
            (from p in _context.Payment
             where p.IDPayment == payment.ID
             select p).ToList().ForEach(x => x.Status = 1);
            _context.SaveChanges();
            return "Success";

        }
        public string UpdateStatusCompleteTheOrther(PaymentUpdate payment)
        {
            (from p in _context.Payment
             where p.IDPayment == payment.ID
             select p).ToList().ForEach(x => x.Status = 2);
            _context.SaveChanges();
            return "Success";
        }
        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            return await _context.Payment.ToListAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payment.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.IDPayment)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.IDPayment }, payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.IDPayment == id);
        }
    }
}
