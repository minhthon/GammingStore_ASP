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
    public class Accounts_APIController : ControllerBase
    {
        private readonly DpContext _context;

        public Accounts_APIController(DpContext context)
        {
            _context = context;
        }

        // GET: api/Accounts_API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
            return await _context.Account.ToListAsync();
        }

        // GET: api/Accounts_API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        public class AccountUpload
        {
            public string Email { get; set; }
            public bool Status { get; set; }
        }

        [HttpPost]
        public string UpdateStatus(AccountUpload account)
        {
            (from p in _context.Account
             where p.Email == account.Email
             select p).ToList().ForEach(x => x.Status = account.Status);
            _context.SaveChanges();
            return "{\"email\":\""+ account.Email+ "\",\"stt\":\"" + account.Status + "\"}";
        }

        // PUT: api/Accounts_API/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAccount(string id, Account account)
        {
            if (id != account.Email)
            {
                return "";
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return "";
                }
                else
                {
                    throw;
                }
            }

            return "{\"email\":" + id + ",\"stt\":\"" + account.Status + "\"}";
        }

        // POST: api/Accounts_API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Account.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccount", new { id = account.Email }, account);
        }

        // DELETE: api/Accounts_API/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAccount(string id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return "";
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return "Success";
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Email == id);
        }
    }
}
