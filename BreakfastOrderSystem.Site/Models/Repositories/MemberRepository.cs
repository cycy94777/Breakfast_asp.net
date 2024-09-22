using BreakfastOrderSystem.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Repositories
{
    public class MemberRepository
    {
       
        private AppDbContext _db;

        public MemberRepository()
        {
            _db = new AppDbContext();
        }

        public IEnumerable<Member> GetMembers()
        {
            var memberData = _db.Members;
            return memberData.ToList();
        }

        public Member GetMember(string memberName)
        {
            return _db.Members.FirstOrDefault(m => m.MemberName == memberName);
        }

        // 獲得被列進黑名單的會員
        public IEnumerable<Member> GetBlacklistedMembers()
        {
            return _db.Members
                           .Where(m => m.IsInBlacklist)
                           .ToList();
        }

        // 將指定會員移出黑名單
        public void UnblockMember(string email)
        {
            var member = _db.Members.FirstOrDefault(m => m.Email == email);
            if (member != null)
            {
                member.IsInBlacklist = false;
                _db.SaveChanges();
            }
        }



    }
}