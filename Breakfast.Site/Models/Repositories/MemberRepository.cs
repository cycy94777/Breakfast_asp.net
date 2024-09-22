using Breakfast.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Breakfast.Site.Models.Repositories
{
    public class MemberRepository
    {
        private AppDbContext _db;

        public MemberRepository()
        {
            _db = new AppDbContext();
        }

        // 獲得所有會員的明細
        public IEnumerable<Member> GetMembers()
        {
            var memberData = _db.Members;
            return memberData.ToList();
        }

        // 依據所蒐尋名字找出該會員明細
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
